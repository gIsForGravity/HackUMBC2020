using HackUMBC.Structs;
using System;

namespace HackUMBC
{
    internal static class TickManager
    {
        private static event Action<Input> OnTick;

        public static void RunTick(Input input) => OnTick(input);

        public static void Register(TickBehaviour tickBehaviour)
        {
            OnTick += tickBehaviour.Tick;
        }
    }
}
