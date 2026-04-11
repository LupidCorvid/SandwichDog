using System;
using UnityEngine;

//Note: this script is specialized for the Player prefab

public class FollowController : MonoBehaviour
{
    [SerializeField] public float forwardOffset; // = 0.2f;
    //public float rotationOffset = 180;
    private float fourLeg_ZOffset = 0.4f;   //Offset to add when player is not standing
    public GameObject targetPosition;           //What the player object should match position of
    public GameObject targetRotation;           //What the player object should match rotation of
    public bool matchXRotation = false;
    public bool matchYRotation = true;
    public bool matchZRotation = false;
    public bool matchXPos;
    public bool matchYPos;
    public bool matchZPos;
    [SerializeField] public bool useRotationOffset;

    float currYOffset = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition != null && targetRotation != null)
        {
            MatchPosition(targetPosition);
            MatchRotation(targetRotation);
        }
    }

    public void ToggleYOffset(bool isStanding)
    {
        if (isStanding) currYOffset = 0f;
        else currYOffset = fourLeg_ZOffset;
    }

    public void MatchPosition(GameObject target)
    {
        Vector3 newPos = target.transform.position;

        if (!matchYPos) newPos.y = gameObject.transform.position.y;
        if (!matchXPos) newPos.x = gameObject.transform.position.x;
        if (!matchZPos) newPos.z = gameObject.transform.position.z;

        //newPos.z -= forwardOffset - currYOffset; //Needs to subtract more when in 4 leg mode
        gameObject.transform.position = newPos;
    }
    public void MatchRotation(GameObject target)
    {
        Vector3 newRot = target.transform.eulerAngles;

        //Locks rotation for specified axes
        if (!matchXRotation) newRot.x = gameObject.transform.eulerAngles.x;
        if (!matchYRotation) newRot.y = gameObject.transform.eulerAngles.y;
        if (!matchZRotation) newRot.z = gameObject.transform.eulerAngles.z;

        //Model always faces wrong way, this combats it
        newRot.y -= 180;

        //Apply rotation
        gameObject.transform.eulerAngles = newRot;
        if (useRotationOffset) ApplyRotationOffset(target);
    }

    //Applies an X and Z positional offset that happens due to rotation
    //Always want a forwardOffset distance away from center of target
    public void ApplyRotationOffset(GameObject target)
    {
        double targetYRotation = target.transform.eulerAngles.y % 360;      //Bounds rotation to 360 in case it's been overshot
        double targetYRotation_radians = targetYRotation * (Math.PI / 180); //Convert angle to radians
        double zOffset = Math.Sin(targetYRotation_radians);                 
        double xOffset = Math.Cos(targetYRotation_radians);

        //Add a certain amount of x and z depending on what angle the target rotated by
        Vector3 newPos = gameObject.transform.position;
        newPos.z += (float)(xOffset) * forwardOffset;
        newPos.x += (float)(zOffset) * forwardOffset;
        gameObject.transform.position = newPos;
    }
}
