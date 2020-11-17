using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;
using System.Collections.Generic;

namespace HackUMBC
{
    public class Host : MonoBehaviour
    {
        private bool ticking = false;
        public int Tick { get; private set; } = 0;

        [SerializeField] private Transform[] NonClientBalls = null;
        private Rigidbody[] NonClientBallRigidbodies;
        [SerializeField] private Transform Player = null;
        [SerializeField] private int maxTicks = 5000;
        private Rigidbody PlayerRigidbody;

        internal static Host singleton { get; private set; }

        NetManager netManager;
        NetPacketProcessor packetProcessor;
        NetDataWriter writer;
        EventBasedNetListener netListener;

        public Structs.Input LastReceivedInput { set { lastReceivedInput = value; } }

        private Structs.Input lastReceivedInput;

        private void Awake()
        {
            Screen.SetResolution(1280, 720, false);
            Physics.autoSimulation = false;

            NonClientBallRigidbodies = new Rigidbody[NonClientBalls.Length];

            for (int i = 0; i < NonClientBalls.Length; i++)
            {
                NonClientBallRigidbodies[i] = NonClientBalls[i].GetComponent<Rigidbody>();
                if (NonClientBallRigidbodies[i] == null) Debug.LogError("Rigidbody null");
            }

            PlayerRigidbody = Player.GetComponent<Rigidbody>();

            inputs.Add(0, new Structs.Input { Forward = false, Backward = false, Left = false, Right = false });
            states.Add(0, CalculateState());

            singleton = this;
        }

        public void StartHost(GameObject canvas)
        {
            canvas.SetActive(false);

            Debug.LogError("starting host");

            netListener = new EventBasedNetListener();
            netListener.PeerConnectedEvent += (client) =>
            {
                Debug.LogError($"Connected to client: {client}");
                Debug.LogError("Sending InitialTickPacket");
                SendPacket(new InitialTickPacket { tick = Tick }, DeliveryMethod.ReliableUnordered);
            };

            netListener.ConnectionRequestEvent += (request) =>
            {
                request.Accept();
            };

            netListener.NetworkReceiveEvent += (peer, reader, deliveryMethod) =>
            {
                packetProcessor.ReadAllPackets(reader);
            };

            packetProcessor = new NetPacketProcessor();
            writer = new NetDataWriter();
            PacketRegistrar.RegisterPackets(packetProcessor);

            netManager = new NetManager(netListener);
            netManager.SimulatePacketLoss = true;
            netManager.SimulateLatency = true;
            netManager.SimulationMaxLatency = 300;
            netManager.SimulationPacketLossChance = 5;
            netManager.Start(12345);

            ticking = true;
        }

        public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, Packet, new()
        {
            if (netManager != null)
            {
                writer.Reset();
                packetProcessor.Write(writer, packet);
                netManager.SendToAll(writer, deliveryMethod);
            }
        }

        public void SendPacketToClient<T>(T packet, int id, DeliveryMethod deliveryMethod) where T : class, Packet, new()
        {
            if (netManager != null)
            {
                writer.Reset();
                packetProcessor.Write(writer, packet);
                netManager.SendToAll(writer, deliveryMethod);
            }
        }

        internal static Dictionary<int, Structs.Input> inputs = new Dictionary<int, Structs.Input>();
        internal static Dictionary<int, Structs.GameState> states = new Dictionary<int, Structs.GameState>();

        private static int earliestTick = 0;

        internal static int resimTick = 0;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!ticking) return;
            Tick += 1;

            netManager.PollEvents();

            if (resimTick < Tick - 1)
            {
                ResetState(resimTick - 1);
                resim(resimTick, Tick);
            }
            else
                resim(Tick, Tick);

            resimTick = Tick;

            if (Tick - maxTicks > earliestTick)
                deleteTicks(earliestTick, Tick - maxTicks);

            earliestTick = Tick - maxTicks;

            SendPacket(HostGameStateOnTickPacket.FromGameState(states[Tick], Tick), DeliveryMethod.Sequenced);
        }

        private void resim(int firstTick, int lastTick)
        {
            for (int i = firstTick; i <= lastTick; i++)
            {
                if (!inputs.ContainsKey(i))
                    inputs.Add(i, inputs[i - 1]);

                TickManager.RunTick(inputs[i]);
                Physics.Simulate(0.02f);

                if (states.ContainsKey(i))
                    states.Remove(i);
                states.Add(i, CalculateState());
            }
        }

        private void deleteTicks(int firstTick, int lastTick)
        {
            for (int i = firstTick; firstTick <= lastTick; i++)
            {
                if (states.ContainsKey(i))
                    states.Remove(i);
                if (inputs.ContainsKey(i))
                    inputs.Remove(i);
            }
        }

        private Structs.GameState CalculateState()
        {
            var locations = new Vector3[NonClientBalls.Length];
            var rotations = new Quaternion[NonClientBalls.Length];
            var velocities = new Vector3[NonClientBallRigidbodies.Length];
            var angularVelocities = new Vector3[NonClientBallRigidbodies.Length];

            for (int i = 0; i < NonClientBalls.Length; i++)
            {
                locations[i] = NonClientBalls[i].position;
                rotations[i] = NonClientBalls[i].rotation;
            }

            for (int i = 0; i < NonClientBallRigidbodies.Length; i++)
            {
                velocities[i] = new Vector3(NonClientBallRigidbodies[i].velocity.x, NonClientBallRigidbodies[i].velocity.y, NonClientBallRigidbodies[i].velocity.z);
                angularVelocities[i] = NonClientBallRigidbodies[i].angularVelocity;
            }

            return new Structs.GameState
            {
                ballLocations = locations,
                ballRotations = rotations,
                playerLocation = Player.position,
                playerRotation = Player.rotation,
                ballVelocities = velocities,
                ballAngularVelocities = angularVelocities,
                playerVelocity = PlayerRigidbody.velocity,
                playerAngularVelocity = PlayerRigidbody.angularVelocity
            };
        }

        private void ResetState(int tick)
        {
            var state = states[tick];
            for (int i = 0; i < state.ballLocations.Length; i++)
            {
                NonClientBalls[i].position = state.ballLocations[i];
                NonClientBalls[i].rotation = state.ballRotations[i];
                NonClientBallRigidbodies[i].velocity = state.ballVelocities[i];
                NonClientBallRigidbodies[i].angularVelocity = state.ballAngularVelocities[i];
            }
            Player.position = state.playerLocation;
            Player.rotation = state.playerRotation;
            PlayerRigidbody.velocity = state.playerVelocity;
            PlayerRigidbody.angularVelocity = state.playerAngularVelocity;
        }
    }
}
