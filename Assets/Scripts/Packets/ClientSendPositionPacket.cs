using UnityEngine;

namespace Packets
{
    public class ClientSendPositionPacket : Packet
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int tick { get; set; }

        private static Transform player;

        private static int lastReceivedTick = -1;

        public static void OnReceive(ClientSendPositionPacket packet)
        {
            if (player == null)
                player = GameObject.Find("Player").transform;

            if (packet.tick > lastReceivedTick)
                player.position = new Vector3(packet.x, packet.y, packet.z);
        }
    }
}
