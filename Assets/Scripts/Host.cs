using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

namespace HackUMBC
{
    public class Host : MonoBehaviour
    {
        private bool ticking = false;
        public int Tick { get; private set; } = 0;

        public Transform[] NonClientBalls;

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

            inputs.Add(0, new Structs.Input { Forward = false, Backward = false, Left = false, Right = false });
            states.Add(0, CalculateState());

            singleton = this;
        }

        public void StartHost()
        {
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
        }

        private void resim(int firstTick, int lastTick)
        {
            for (int i = firstTick; i <= lastTick; i++)
            {
                if (!inputs.ContainsKey(i))
                    inputs.Add(i, inputs[i - 1]);

                TickManager.RunTick(inputs[i]);
                Physics.Simulate(0.02f);
                CalculateState();
            }
        }

        private Structs.GameState CalculateState()
        {
            var locations = new Vector3[NonClientBalls.Length];
            var rotations = new Quaternion[NonClientBalls.Length];

            return new Structs.GameState
            {
                ballLocations = locations,
                ballRotations = rotations
            };
        }

        private void ResetState(int tick)
        {
            var state = states[tick];
            for (int i = 0; i < state.ballLocations.Length; i++)
            {
                NonClientBalls[i].position = state.ballLocations[i];
                NonClientBalls[i].rotation = state.ballRotations[i];
            }
        }
    }
}
