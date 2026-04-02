using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

//Attach this script to the handle and zero out rotation if it exceeds a certain value
//DO this for lower and upper bound of rotation

public class cabinetRotationBounds : MonoBehaviour
{
    [SerializeField] float upperBound = 100; //cabinets: 100
    [SerializeField] float lowerBound = 0; //cabinets: 0
    private float epsilon = 0.05f;

    enum axes { X, Y, Z };
    [SerializeField] axes rotationAxis;
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
        //Make sure the object doesn't rotate in any other direction (avoids weird tilt thing)
        gameObject.transform.localEulerAngles = GetRotationAxis(gameObject.transform.localEulerAngles.y);

        //Snap the object if it goes beyond a certain threshold
        if (GetCurrLocalEulerAngles() < lowerBound - epsilon/* || gameObject.transform.localEulerAngles.y > upperBound + epsilon*/)
        {
            //Change which angle to snap at depending on which way the door is opening
            float snapLoc = 0f;
            if (GetCurrRBRotation() < 0) snapLoc = lowerBound;
            else snapLoc = upperBound;

            //Put the rotation at either the upper or lower bound
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            gameObject.transform.localEulerAngles = GetRotationAxis(snapLoc);
        }
    }


    //Some objects need to rotate on differing axes
    //One degree of freedom
    Vector3 GetRotationAxis(float value)
    {
        switch (rotationAxis)
        {
            case axes.X:
                return new Vector3(value, 0, 0);
            case axes.Y:
                return new Vector3(0, value, 0);
            case axes.Z:
                return new Vector3(0, 0, value);
            default:
                return Vector3.zero;
        }
    }

    //Checking the current value of the rigidbody axis that rotates
    float GetCurrRBRotation()
    {
        switch (rotationAxis)
        {
            case axes.X:
                return rb.angularVelocity.x;
            case axes.Y:
                return rb.angularVelocity.y;
            case axes.Z:
                return rb.angularVelocity.z;
            default:
                return rb.angularVelocity.y;
        }
    }
    
    float GetCurrLocalEulerAngles()
    {
        switch (rotationAxis)
        {
            case axes.X:
                return gameObject.transform.localEulerAngles.x;
            case axes.Y:
                return gameObject.transform.localEulerAngles.y;
            case axes.Z:
                return gameObject.transform.localEulerAngles.z;
            default:
                return gameObject.transform.localEulerAngles.y;
        }
    }
}
