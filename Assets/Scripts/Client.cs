using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;
using System.Collections.Generic;

namespace HackUMBC
{
    public class Client : MonoBehaviour
    {
        public static bool ticking { get; set; } = false;
        private bool connected = false;
        public static int Tick { get; set; } = 0;

        NetManager netManager;
        NetPacketProcessor packetProcessor;
        NetDataWriter writer;
        EventBasedNetListener netListener;

        private void Awake()
        {
            Screen.SetResolution(1280, 720, false);
        }

        public void StartClient()
        {
            Debug.LogError("starting client");

            netListener = new EventBasedNetListener();
            netListener.PeerConnectedEvent += (server) =>
            {
                Debug.LogError($"Connected to server: {server}");
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
            netManager.Start();
            netManager.Connect("localhost", 12345, "");

            connected = true;
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

        public Transform player;

        static internal int lastTickReceived = 0;

        private Dictionary<int, Structs.Input> inputs = new Dictionary<int, Structs.Input>();

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!connected) return;
            netManager.PollEvents();

            if (!ticking) return;
            Tick += 1;

            /*var position = player.position;
            var packet = new ClientSendPositionPacket
            {
                x = position.x,
                y = position.y,
                z = position.z,
                tick = Tick
            };
            SendPacket(packet, DeliveryMethod.Unreliable);*/

            var input = new Structs.Input
            {
                Forward = Input.GetKey(KeyCode.W),
                Left = Input.GetKey(KeyCode.A),
                Backward = Input.GetKey(KeyCode.S),
                Right = Input.GetKey(KeyCode.D)
            };

            inputs.Add(Tick, input);

            TickManager.RunTick(input);
            Physics.Simulate(0.02f);

            {
                int repeats = Tick - lastTickReceived;

                var inputPacket = new ClientInputPacket
                {
                    inputs = new Structs.Input[repeats],
                    lastTick = Tick
                };

                for (int i = 0; i < repeats; i++)
                {
                    if (inputs.ContainsKey(lastTickReceived + i))
                        inputPacket.inputs[i] = inputs[lastTickReceived + i];
                    else continue;
                }

                SendPacket(inputPacket, DeliveryMethod.Sequenced);

                Debug.Log("Input Packet Sent");
            }

            Debug.Log("lastTickReceived = " + lastTickReceived);
        }
    }
}
