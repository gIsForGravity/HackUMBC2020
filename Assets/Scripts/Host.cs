using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;

namespace HackUMBC
{
    public class Host : MonoBehaviour
    {
        private bool ticking = false;
        public int Tick { get; private set; } = 0;

        NetManager netManager;
        NetPacketProcessor packetProcessor;
        NetDataWriter writer;
        EventBasedNetListener netListener;

        private void Awake()
        {
            Screen.SetResolution(1280, 720, false);
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

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!ticking) return;
            Tick += 1;

            netManager.PollEvents();
        }
    }
}
