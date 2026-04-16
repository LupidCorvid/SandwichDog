using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//Note: this script is specialized for the Player prefab

public class FollowController : MonoBehaviour
{
    [SerializeField] public float forwardOffset; // = 0.2f;
    public float cameraOffsetAmount;
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
    public bool adjustForCamera;
    public GameObject cameraGO;
    public float lowerOffshoot; //Y axis, lower bound for rotating to follow camera rotation
    public float upperOffshoot; //Y axis, upper bound for rotating to follow camera rotation
    [SerializeField] public bool useRotationOffset;

    float currYOffset = 0f;
    enum RelationToCam { IN_BOUNDS, OUT_BOUNDS };
    enum RotDirection { CLOCKWISE, COUNTER_CLOCKWISE};
    private RotDirection directionToRotate = RotDirection.CLOCKWISE;
    private RelationToCam prevState = RelationToCam.IN_BOUNDS;
    private RelationToCam currState = RelationToCam.IN_BOUNDS;


    // Update is called once per frame
    void Update()
    {
        if (targetPosition != null && targetRotation != null)
        {
            MatchPosition(targetPosition);
            MatchRotation(targetRotation);
        }
    }

    //Done when switching from 2 to 4 legs
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

        //Adjust for camera offshoot
        if (adjustForCamera) newRot = adjustForCameraOffshoot(newRot);

        //Apply rotation
        gameObject.transform.eulerAngles = newRot;
        if (useRotationOffset) ApplyRotationOffset(target, forwardOffset);
    }

    //If the camera local rotation overshoots a certain Y value, turn the entire body so that the player cant see their own head
    public Vector3 adjustForCameraOffshoot(Vector3 in_vector)
    {
        float cameraY = cameraGO.transform.localEulerAngles.y;

        //If camera rotation is currently out of bounds of the offshoots
        if (cameraY > lowerOffshoot && cameraY < upperOffshoot)
        {
            //If freshly OOB, determine which way to turn the model
            if(prevState == RelationToCam.IN_BOUNDS)
            {
                if (cameraY < 180) directionToRotate = RotDirection.CLOCKWISE;
                else directionToRotate = RotDirection.COUNTER_CLOCKWISE;
            }

            //Rotate the model
            if(directionToRotate == RotDirection.CLOCKWISE)
            {
                float difference = Math.Abs(cameraY - lowerOffshoot);
                in_vector.y += difference;
            }
            else
            {
                float difference = Math.Abs(cameraY - upperOffshoot);
                in_vector.y -= difference;
            }
            if (useRotationOffset) ApplyRotationOffset(cameraGO, cameraOffsetAmount);
            prevState = currState;
            currState = RelationToCam.OUT_BOUNDS;
        }
        //If in bounds
        else
        {
            prevState = currState;
            currState = RelationToCam.IN_BOUNDS;
        }
        return in_vector;
    }

    //Applies an X and Z positional offset that happens due to rotation
    //Always want a forwardOffset distance away from center of target
    public void ApplyRotationOffset(GameObject target, float offsetAmt)
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
