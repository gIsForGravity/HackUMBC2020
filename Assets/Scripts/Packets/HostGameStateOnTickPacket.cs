using HackUMBC.Packets;
using HackUMBC.Structs;
using UnityEngine;

namespace HackUMBC
{
    public class HostGameStateOnTickPacket : Packet
    {
        public float[] ballLocationsX;
        public float[] ballLocationsY;
        public float[] ballLocationsZ;

        public float[] ballRotationsX;
        public float[] ballRotationsY;
        public float[] ballRotationsZ;
        public float[] ballRotationsW;

        public float playerLocationX;
        public float playerLocationY;
        public float playerLocationZ;

        public float playerRotationX;
        public float playerRotationY;
        public float playerRotationZ;
        public float playerRotationW;

        public float[] ballVelocitiesX;
        public float[] ballVelocitiesY;
        public float[] ballVelocitiesZ;

        public float[] ballAngularVelocitiesX;
        public float[] ballAngularVelocitiesY;
        public float[] ballAngularVelocitiesZ;

        public float playerVelocityX;
        public float playerVelocityY;
        public float playerVelocityZ;

        public float playerAngularVelocityX;
        public float playerAngularVelocityY;
        public float playerAngularVelocityZ;
        public float playerAngularVelocityW;

        public int tick;

        public static GameState ToGameState(HostGameStateOnTickPacket packet)
        {
            var length = packet.ballLocationsX.Length;

            GameState state = new GameState
            {
                ballLocations = new Vector3[length],
                ballRotations = new Quaternion[length],
                playerLocation = new Vector3(packet.playerLocationX, packet.playerLocationY, packet.playerLocationZ),
                playerRotation = new Quaternion(packet.playerRotationX, packet.playerRotationY, packet.playerRotationZ, packet.playerRotationW),
                ballVelocities = new Vector3[length],
                ballAngularVelocities = new Vector3[length],
                playerVelocity = new Vector3(packet.playerVelocityX, packet.playerVelocityY, packet.playerVelocityZ),
                playerAngularVelocity = new Vector3(packet.playerAngularVelocityX, packet.playerAngularVelocityY, packet.playerAngularVelocityZ),
            };

            for (int i = 0; i < packet.ballLocationsX.Length; i++)
            {
                state.ballLocations[i] = new Vector3(packet.ballLocationsX[i], packet.ballLocationsY[i], packet.ballLocationsZ[i]);
                state.ballRotations[i] = new Quaternion(packet.ballRotationsX[i], packet.ballRotationsY[i], packet.ballRotationsZ[i], packet.ballRotationsW[i]);
                state.ballVelocities[i] = new Vector3(packet.ballVelocitiesX[i], packet.ballVelocitiesY[i], packet.ballVelocitiesZ[i]);
                state.ballAngularVelocities[i] = new Vector3(packet.ballAngularVelocitiesX[i], packet.ballAngularVelocitiesX[i], packet.ballAngularVelocitiesX[i]);
            }

            return state;
        }

        public static HostGameStateOnTickPacket FromGameState(GameState state, int tick)
        {
            var length = state.ballLocations.Length;

            var packet = new HostGameStateOnTickPacket
            {
                ballLocationsX = new float[length],
                ballLocationsY = new float[length],
                ballLocationsZ = new float[length],
                ballRotationsX = new float[length],
                ballRotationsY = new float[length],
                ballRotationsZ = new float[length],
                ballRotationsW = new float[length],
                playerLocationX = state.playerLocation.x,
                playerLocationY = state.playerLocation.y,
                playerLocationZ = state.playerLocation.z,
                playerRotationX = state.playerRotation.x,
                playerRotationY = state.playerRotation.y,
                playerRotationZ = state.playerRotation.z,
                ballVelocitiesX = new float[length],
                ballVelocitiesY = new float[length],
                ballVelocitiesZ = new float[length],
                ballAngularVelocitiesX = new float[length],
                ballAngularVelocitiesY = new float[length],
                ballAngularVelocitiesZ = new float[length],
                playerVelocityX = state.playerVelocity.x,
                playerVelocityY = state.playerVelocity.y,
                playerVelocityZ = state.playerVelocity.z,
                playerAngularVelocityX = state.playerAngularVelocity.x,
                playerAngularVelocityY = state.playerAngularVelocity.y,
                playerAngularVelocityZ = state.playerAngularVelocity.z,
                tick = tick
            };

            for (int i = 0; i < length; i++)
            {
                packet.ballLocationsX[i] = state.ballLocations[i].x;
                packet.ballLocationsY[i] = state.ballLocations[i].y;
                packet.ballLocationsZ[i] = state.ballLocations[i].z;
                packet.ballRotationsX[i] = state.ballRotations[i].x;
                packet.ballRotationsY[i] = state.ballRotations[i].y;
                packet.ballRotationsZ[i] = state.ballRotations[i].z;
                packet.ballVelocitiesX[i] = state.ballVelocities[i].x;
                packet.ballVelocitiesY[i] = state.ballVelocities[i].y;
                packet.ballVelocitiesZ[i] = state.ballVelocities[i].z;
                packet.ballAngularVelocitiesX[i] = state.ballAngularVelocities[i].x;
                packet.ballAngularVelocitiesY[i] = state.ballAngularVelocities[i].y;
                packet.ballAngularVelocitiesZ[i] = state.ballAngularVelocities[i].z;
            }

            return packet;
        }

        public static void OnReceive(HostGameStateOnTickPacket packet)
        {
            Client.LoadState(ToGameState(packet), packet.tick);
        }
    }
}
