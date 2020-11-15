using LiteNetLib.Utils;
using UnityEngine;

namespace HackUMBC
{
    public static class QuaternionSerializer
    {
        public static void Serialize(NetDataWriter writer, Quaternion quaternion)
        {
            writer.Put(quaternion.x);
            writer.Put(quaternion.y);
            writer.Put(quaternion.z);
            writer.Put(quaternion.w);
        }

        public static Quaternion Deserialize(NetDataReader reader)
        {
            return new Quaternion(reader.GetFloat(), reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
        }
    }
}
