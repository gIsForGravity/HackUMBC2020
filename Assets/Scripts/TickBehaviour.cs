using HackUMBC.Structs;
using UnityEngine;

namespace HackUMBC
{
    public abstract class TickBehaviour : MonoBehaviour
    {
        void Awake()
        {
            TickManager.Register(this);
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public abstract void Tick(Structs.Input input);
    }
}
