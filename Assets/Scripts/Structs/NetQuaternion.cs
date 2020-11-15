using LiteNetLib.Utils;
using UnityEngine;

namespace HackUMBC.Structs
{
    public struct NetQuaternion : INetSerializable
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetFloat();
            y = reader.GetFloat();
            z = reader.GetFloat();
            w = reader.GetFloat();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
            writer.Put(z);
            writer.Put(w);
        }

        public static implicit operator Quaternion(NetQuaternion v) => new Quaternion(v.x, v.y, v.z, v.w);
        public static implicit operator NetQuaternion(Quaternion v) => new NetQuaternion { x = v.x, y = v.y, z = v.z, w = v.w };
    }
}
