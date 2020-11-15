using UnityEngine;

namespace HackUMBC.Structs
{
    public struct GameState
    {
        public Vector3[] ballLocations;
        public Quaternion[] ballRotations;
        public Vector3[] ballVelocities;
        public Vector3[] ballAngularVelocities;
    }
}
