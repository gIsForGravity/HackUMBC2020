using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;

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

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!connected) return;
            netManager.PollEvents();

            if (!ticking) return;
            Tick += 1;

            var position = player.position;
            var packet = new ClientSendPositionPacket
            {
                x = position.x,
                y = position.y,
                z = position.z,
                tick = Tick
            };
            SendPacket(packet, DeliveryMethod.Unreliable);
        }
    }
}
