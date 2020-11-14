using LiteNetLib.Utils;
using HackUMBC.Packets;

namespace HackUMBC
{
    public static class PacketRegistrar
    {
        public static void RegisterPackets(NetPacketProcessor processor)
        {
            processor.SubscribeReusable<ClientSendPositionPacket>(ClientSendPositionPacket.OnReceive);
            processor.SubscribeReusable<InitialTickPacket>(InitialTickPacket.OnReceive);
        }
    }
}
