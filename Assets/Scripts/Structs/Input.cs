using LiteNetLib.Utils;

namespace HackUMBC.Structs
{
    public struct Input : INetSerializable
    {
        public bool Forward;
        public bool Left;
        public bool Right;
        public bool Backward;

        public void Deserialize(NetDataReader reader)
        {
            Forward = reader.GetBool();
            Left = reader.GetBool();
            Right = reader.GetBool();
            Backward = reader.GetBool();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Forward);
            writer.Put(Left);
            writer.Put(Right);
            writer.Put(Backward);
        }
    }
}