using UnityEngine;

//Attach this script to the handle and zero out rotation if it exceeds a certain value
//DO this for lower and upper bound of rotation

public class cabinetRotationBounds : MonoBehaviour
{
    private float upperBound = 170;
    private float lowerBound = 0;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lowerBound
        if(gameObject.transform.eulerAngles.y < lowerBound)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, lowerBound, gameObject.transform.eulerAngles.z);
            print("attempt");
        }

        //upperBound
    }
}
