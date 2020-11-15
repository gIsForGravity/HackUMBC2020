using UnityEngine;

namespace HackUMBC.Packets
{
    public class InitialTickPacket : Packet
    {
        public int tick { get; set; }

        public static void OnReceive(InitialTickPacket packet)
        {
            Debug.Log($"Tick packet received. Tick: {packet.tick}");
            Client.Tick = packet.tick;
            Client.lastTickReceived = packet.tick;
            Client.ticking = true;
        }
    }
}
