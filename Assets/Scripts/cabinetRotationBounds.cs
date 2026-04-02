using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

//Attach this script to the handle and zero out rotation if it exceeds a certain value
//DO this for lower and upper bound of rotation

public class cabinetRotationBounds : MonoBehaviour
{
    private float upperBound = 100;
    private float lowerBound = 0;
    private float epsilon = 0.05f;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 3;
        gameObject.transform.localEulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);

        if (gameObject.transform.localEulerAngles.y < lowerBound - epsilon/* || gameObject.transform.localEulerAngles.y > upperBound + epsilon*/)
        {
            //Change which angle to snap at depending on which way the door is opening
            float snapLoc = 0f;
            if (rb.angularVelocity.y < 0) snapLoc = lowerBound;
            else snapLoc = upperBound;

            //Put the rotation at either the upper or lower bound
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            gameObject.transform.localEulerAngles = new Vector3(0, snapLoc, 0);
        }
    }
}
