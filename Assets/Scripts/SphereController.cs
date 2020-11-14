using UnityEngine;

namespace HackUMBC
{
    public class SphereController : TickBehaviour
    {
        Rigidbody rb;
        public float Force = 1f;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 force = new Vector3();
            if (Input.GetKey(KeyCode.W))
                force += Vector3.forward * Force;
            if (Input.GetKey(KeyCode.S))
                force += Vector3.back * Force;
            if (Input.GetKey(KeyCode.A))
                force += Vector3.left * Force;
            if (Input.GetKey(KeyCode.D))
                force += Vector3.right * Force;

            rb.AddForce(force);
            //if (rb.velocity.magnitude)
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}
