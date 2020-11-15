using LiteNetLib.Utils;
using UnityEngine;

namespace HackUMBC.Structs
{
    public struct NetVector3 : INetSerializable
    {
        public float x;
        public float y;
        public float z;

        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetFloat();
            y = reader.GetFloat();
            z = reader.GetFloat();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
            writer.Put(z);
        }

        public static implicit operator Vector3(NetVector3 v) => new Vector3(v.x, v.y, v.z);
        public static implicit operator NetVector3(Vector3 v) => new NetVector3 { x = v.x, y = v.y, z = v.z };
    }
}
