using LiteNetLib.Utils;
using UnityEngine;

namespace HackUMBC.Structs
{
    public struct GameState
    {
        public Vector3[] ballLocations;
        public Quaternion[] ballRotations;
        public Vector3 playerLocation;
        public Quaternion playerRotation;
        public Vector3[] ballVelocities;
        public Vector3[] ballAngularVelocities;
        public Vector3 playerVelocity;
        public Vector3 playerAngularVelocity;
    }
}
