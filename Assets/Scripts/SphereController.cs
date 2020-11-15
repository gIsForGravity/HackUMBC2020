using UnityEngine;

namespace HackUMBC
{
    public class SphereController : TickBehaviour
    {
        Rigidbody rb;
        public float Force = 1f;

        protected override void OnAwake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            /*Vector3 force = new Vector3();
            if (Input.GetKey(KeyCode.W))
                force += Vector3.forward * Force;
            if (Input.GetKey(KeyCode.S))
                force += Vector3.back * Force;
            if (Input.GetKey(KeyCode.A))
                force += Vector3.left * Force;
            if (Input.GetKey(KeyCode.D))
                force += Vector3.right * Force;

            rb.AddForce(force);*/
        }

        public override void Tick(Structs.Input input)
        {
            Vector3 force = new Vector3();
            if (input.Forward)
                force += Vector3.forward * Force;
            if (input.Backward)
                force += Vector3.back * Force;
            if (input.Left)
                force += Vector3.left * Force;
            if (input.Right)
                force += Vector3.right * Force;

            rb.AddForce(force);
        }
    }
}
