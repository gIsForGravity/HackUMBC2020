using HackUMBC.Packets;
using HackUMBC.Structs;
using UnityEngine;

namespace HackUMBC
{
    public class HostGameStateOnTickPacket : Packet
    {
        public NetVector3[] ballLocations;
        public NetQuaternion[] ballRotations;
        public NetVector3 playerLocation;
        public NetQuaternion playerRotation;
        public NetVector3[] ballVelocities;
        public NetVector3[] ballAngularVelocities;
        public NetVector3 playerVelocity;
        public NetVector3 playerAngularVelocity;
        public int tick;

        public static GameState ToGameState(HostGameStateOnTickPacket packet)
        {
            var length = packet.ballLocations.Length;

            GameState state = new GameState
            {
                ballLocations = new Vector3[length],
                ballRotations = new Quaternion[length],
                playerLocation = packet.playerLocation,
                playerRotation = packet.playerRotation,
                ballVelocities = new Vector3[length],
                ballAngularVelocities = new Vector3[length],
                playerVelocity = packet.playerVelocity,
                playerAngularVelocity = packet.playerAngularVelocity
            };

            for (int i = 0; i < packet.ballLocations.Length; i++)
            {
                state.ballLocations[i] = packet.ballLocations[i];
                state.ballRotations[i] = packet.ballRotations[i];
                state.ballVelocities[i] = packet.ballVelocities[i];
                state.ballAngularVelocities[i] = packet.ballAngularVelocities[i];
            }

            return state;
        }

        public static HostGameStateOnTickPacket FromGameState(GameState state, int tick)
        {
            var length = state.ballLocations.Length;

            var packet = new HostGameStateOnTickPacket
            {
                ballLocations = new NetVector3[length],
                ballRotations = new NetQuaternion[length],
                playerLocation = state.playerLocation,
                playerRotation = state.playerRotation,
                ballVelocities = new NetVector3[length],
                ballAngularVelocities = new NetVector3[length],
                playerVelocity = state.playerVelocity,
                playerAngularVelocity = state.playerAngularVelocity,
                tick = tick
            };

            for (int i = 0; i < packet.ballLocations.Length; i++)
            {
                packet.ballLocations[i] = state.ballLocations[i];
                packet.ballRotations[i] = state.ballRotations[i];
                packet.ballVelocities[i] = state.ballVelocities[i];
                packet.ballAngularVelocities[i] = state.ballAngularVelocities[i];
            }

            return packet;
        }

        public static void OnReceive(HostGameStateOnTickPacket packet)
        {
            Client.LoadState(ToGameState(packet), packet.tick);
        }
    }
}
