using UnityEngine;

public class ClampVelocity : MonoBehaviour
{
    public float maxVel;
    Rigidbody rb;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVel);
        //if (rb.linearVelocity.magnitude > .005f) print(rb.linearVelocity.magnitude);
    }
}
