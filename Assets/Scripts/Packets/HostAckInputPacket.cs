using HackUMBC.Structs;

namespace HackUMBC.Packets
{
    public class HostAckInputPacket : Packet
    {
        public int tickReceived { get; set; }

        public static void OnReceive(HostAckInputPacket packet)
        {
            Client.lastTickReceived = packet.tickReceived;
        }
    }
}
