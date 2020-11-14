using LiteNetLib.Utils;
using Packets;
using System;

public static class PacketRegistrar
{
    public static void RegisterPackets(NetPacketProcessor processor)
    {
        processor.SubscribeReusable<ClientSendPositionPacket>(ClientSendPositionPacket.OnReceive);
        processor.SubscribeReusable<InitialTickPacket>(InitialTickPacket.OnReceive);
    }
}
