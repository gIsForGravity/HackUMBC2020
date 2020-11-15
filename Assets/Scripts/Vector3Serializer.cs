using LiteNetLib.Utils;
using UnityEngine;

namespace HackUMBC
{
    public static class Vector3Serializer
    {
        public static void Serialize(NetDataWriter writer, Vector3 vector3)
        {
            writer.Put(vector3.x);
            writer.Put(vector3.y);
            writer.Put(vector3.z);
        }

        public static Vector3 Deserialize(NetDataReader reader)
        {
            return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
        }
    }
}
