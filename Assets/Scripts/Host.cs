using LiteNetLib;
using LiteNetLib.Utils;
using HackUMBC.Packets;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace HackUMBC
{
    public class Host : MonoBehaviour
    {
        private bool ticking = false;
        public int Tick { get; private set; } = 0;

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

        internal static int resimTick = 0;

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!ticking) return;
            Tick += 1;

            netManager.PollEvents();

            if (resimTick < Tick - 1)
                resim(resimTick, Tick);
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
            }
        }
    }
}
