using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


//This script is specialized for the Stove Dials

public class StoveDial : MonoBehaviour
{
    public GameObject cookingArea;
    public GameObject flames;
    public GameObject indicator;

    float lowerBound = 0;
    float upperBound = 90;
    float maxRotation = 180;
    float errorWindow = 1.5f; //How much extra room to give in degrees so it's not liiking for exactly one value
    private float startingRotationZ = 0;
    bool burnerOn = false;
    public bool rotationEnabled = false;

    void Awake()
    {
        cookingArea.GetComponent<CookCollider>().enabled = false;
        flames.SetActive(false);
    }
    
    void FixedUpdate()
    {
        if (rotationEnabled)
        {
            float currentRotation = transform.localEulerAngles.z % 360;
            KeepDialInBounds(currentRotation);
            CheckToTurnOnStove(currentRotation);
        }
    }

    //Makes sure the dial doesn't turn too far
    //TODO: test this
    void KeepDialInBounds(float curr)
    {
        //Undershot (wrap around from 360)
        if (curr > 360 - errorWindow*2) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, lowerBound);
        
        //Overshot
        else if (curr > maxRotation) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, maxRotation);
    }

    void CheckToTurnOnStove(float curr)
    {
        //Turn on burner
        if(curr > (upperBound - errorWindow) && curr < (360 - errorWindow*4) && !burnerOn)
        {
            burnerOn = true;
            cookingArea.GetComponent<CookCollider>().enabled = true;
            flames.SetActive(true);
            indicator.GetComponent<MeshRenderer>().material = indicator.GetComponent<MeshRenderer>().materials[1];
        }
        //Turn off burner
        if(curr < lowerBound + errorWindow && burnerOn)
        {
            burnerOn = false;
            cookingArea.GetComponent<CookCollider>().enabled = false;
            flames.SetActive(false);
            indicator.GetComponent<MeshRenderer>().material = indicator.GetComponent<MeshRenderer>().materials[0];
        }
    }

    public void ToggleRotationOn(SelectEnterEventArgs args)
    {
        rotationEnabled = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
    }
    public void ToggleRotationOff(SelectExitEventArgs args)
    {
        rotationEnabled = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
