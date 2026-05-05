using UnityEngine;


//This script is specialized for the Stove Dials

public class StoveDial : MonoBehaviour
{
    public GameObject cookingArea;
    public GameObject flames;
    float lowerBound = 0;
    float upperBound = 90;
    float errorWindow = 3; //How much extra room to give in degrees so it's not liiking for exactly one value
    private float startingRotationZ = 0;
    bool burnerOn = false;

    void Awake()
    {
        cookingArea.GetComponent<CookCollider>().enabled = false;
        flames.SetActive(false);
    }
    
    void FixedUpdate()
    {
        float currentRotation = Mathf.Abs(transform.localEulerAngles.z - startingRotationZ) % 360;
        //KeepDialInBounds(currentRotation);
        CheckToTurnOnStove(currentRotation);
    }

    //Makes sure the dial doesn't turn too far
    void KeepDialInBounds(float curr)
    {
        if (curr > upperBound - errorWindow) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.z, gameObject.transform.localEulerAngles.z, upperBound - errorWindow);
        if (curr < lowerBound + errorWindow) gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.z, gameObject.transform.localEulerAngles.z, upperBound + errorWindow);
    }

    void CheckToTurnOnStove(float curr)
    {
        //Turn on burner
        if(curr > upperBound - errorWindow && !burnerOn)
        {
            burnerOn = true;
            cookingArea.GetComponent<CookCollider>().enabled = true;
            flames.SetActive(true);
        }
        //Turn off burner
        if(curr < lowerBound + errorWindow && burnerOn)
        {
            burnerOn = false;
            cookingArea.GetComponent<CookCollider>().enabled = false;
            flames.SetActive(false);
        }
    }
}
