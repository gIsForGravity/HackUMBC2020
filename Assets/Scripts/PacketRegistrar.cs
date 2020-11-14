using LiteNetLib.Utils;
using HackUMBC.Packets;
using HackUMBC.Structs;

namespace HackUMBC
{
    public static class PacketRegistrar
    {
        public static void RegisterPackets(NetPacketProcessor processor)
        {
            processor.RegisterNestedType<Input>();

            processor.SubscribeReusable<ClientSendPositionPacket>(ClientSendPositionPacket.OnReceive);
            processor.SubscribeReusable<InitialTickPacket>(InitialTickPacket.OnReceive);
            processor.SubscribeReusable<ClientInputPacket>(ClientInputPacket.OnReceive);
            processor.SubscribeReusable<HostAckInputPacket>(HostAckInputPacket.OnReceive);
        }
    }
}
