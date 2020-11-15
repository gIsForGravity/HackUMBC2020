using HackUMBC.Packets;
using HackUMBC.Structs;
using UnityEngine;

namespace HackUMBC
{
    public class HostGameStateOnTickPacket : Packet
    {
        public Vector3[] ballLocations;
        public Quaternion[] ballRotations;
        public Vector3 playerLocation;
        public Quaternion playerRotation;
        public Vector3[] ballVelocities;
        public Vector3[] ballAngularVelocities;
        public Vector3 playerVelocity;
        public Vector3 playerAngularVelocity;
        public int tick;

        public static GameState ToGameState(HostGameStateOnTickPacket packet)
        {
            return new GameState
            {
                ballLocations = packet.ballLocations,
                ballRotations = packet.ballRotations,
                playerLocation = packet.playerLocation,
                playerRotation = packet.playerRotation,
                ballVelocities = packet.ballVelocities,
                ballAngularVelocities = packet.ballAngularVelocities,
                playerVelocity = packet.playerVelocity,
                playerAngularVelocity = packet.playerAngularVelocity
            };
        }

        public static HostGameStateOnTickPacket FromGameState(GameState state, int tick)
        {
            return new HostGameStateOnTickPacket
            {
                ballLocations = state.ballLocations,
                ballRotations = state.ballRotations,
                playerLocation = state.playerLocation,
                playerRotation = state.playerRotation,
                ballVelocities = state.ballVelocities,
                ballAngularVelocities = state.ballAngularVelocities,
                playerVelocity = state.playerVelocity,
                playerAngularVelocity = state.playerAngularVelocity,
                tick = tick
            };
        }

        public static void OnReceive(HostGameStateOnTickPacket packet)
        {
            //Client.LoadState(ToGameState(packet), packet.tick);
        }
    }
}
