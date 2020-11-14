﻿using System;

public static class TickManager
{
    private static event Action OnTick;

    public static void Register(TickBehaviour tickBehaviour)
    {
        OnTick += tickBehaviour.Tick;
    }
}