using UnityEngine;

namespace HackUMBC
{
    public abstract class TickBehaviour : MonoBehaviour
    {
        void Awake()
        {

            OnAwake();
        }

        protected virtual void OnAwake() { }

        public abstract void Tick();
    }
}
