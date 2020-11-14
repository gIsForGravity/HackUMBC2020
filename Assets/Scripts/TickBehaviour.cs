using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TickBehaviour : MonoBehaviour
{
    void Awake()
    {
        
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public abstract void Tick();
}
