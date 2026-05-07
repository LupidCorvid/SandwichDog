using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Linq;


//This script is specialized for the Stove Dials

public class StoveDial : MonoBehaviour
{
    public GameObject cookingArea;
    public GameObject flames;
    public GameObject indicator;

    float lowerBound = 0;
    float upperBound = 90;
    float maxRotation = 180;
    float errorWindow = 10f; //How much extra room to give in degrees so it's not liiking for exactly one value
    private float startingRotationZ = 0;
    bool burnerOn = false;
    public bool rotationEnabled = false;

    List<Material> materials;

    void Awake()
    {
        cookingArea.GetComponent<CookCollider>().enabled = false;
        flames.SetActive(false);

        materials = indicator.GetComponent<MeshRenderer>().materials.ToList();
        materials.Insert(0, indicator.GetComponent<MeshRenderer>().materials[0]);
        materials.Insert(1, indicator.GetComponent<MeshRenderer>().materials[1]);
    }

    void FixedUpdate()
    {
        if (rotationEnabled)
        {
            float currentRotation = transform.localEulerAngles.z % 360; //Bound to be within 360 degrees, including negative numbers

            //KeepDialInBounds(currentRotation);
            CheckToTurnOnStove(currentRotation);
        }
    }

    //Makes sure the dial doesn't turn too far
    //TODO: test this
    void KeepDialInBounds(float curr)
    {
        //Undershot (wrap around from 360)
        if (curr > 360 - errorWindow * 5 && curr < 360 - errorWindow * 2) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, lowerBound - .5f);

        //Overshot
        else if (curr > maxRotation) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, maxRotation - 1);
    }

    void CheckToTurnOnStove(float curr)
    {
        //Turn on burner
        //180 +- error
        if (curr > (upperBound - errorWindow) && curr < (upperBound + errorWindow) && !burnerOn)
        {
            burnerOn = true;
            cookingArea.GetComponent<CookCollider>().enabled = true;
            flames.SetActive(true);
            indicator.GetComponent<MeshRenderer>().material = materials[1];

            //indicator.GetComponent<MeshRenderer>().material = indicator.GetComponent<MeshRenderer>().materials[1];
        }

        //Turn off burner
        // 0 +- error
        if (curr < (lowerBound + errorWindow) || curr > (360 - errorWindow) && burnerOn)
        {
            burnerOn = false;
            cookingArea.GetComponent<CookCollider>().enabled = false;
            flames.SetActive(false);
            indicator.GetComponent<MeshRenderer>().material = materials[0];
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
