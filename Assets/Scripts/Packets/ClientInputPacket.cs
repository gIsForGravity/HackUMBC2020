using HackUMBC.Structs;

namespace HackUMBC.Packets
{
    public class ClientInputPacket : Packet
    {
        public Input[] inputs { get; set; }

        public int lastTick { get; set; }

        private static int lastArrivedTick = 0;

        public static void OnReceive(ClientInputPacket packet)
        {
            int start;
            var firstArrayTick = packet.lastTick - packet.inputs.Length;

            if (firstArrayTick < lastArrivedTick)
                start = lastArrivedTick - firstArrayTick;
            else
                start = 0;

            if (!(start < 0))
            {
                for (int i = start; i < packet.inputs.Length; i++)
                {
                    if (Host.inputs.ContainsKey(firstArrayTick + i + 1))
                        Host.inputs.Remove(firstArrayTick + i + 1);
                    Host.inputs.Add(firstArrayTick + i + 1, packet.inputs[i]);
                }
                Host.resimTick = lastArrivedTick + 1;
                lastArrivedTick = packet.lastTick;
            }

            Host.singleton.SendPacket(new HostAckInputPacket { tickReceived = lastArrivedTick }, LiteNetLib.DeliveryMethod.Sequenced);
        }
    }
}
