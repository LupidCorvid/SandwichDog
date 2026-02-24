using UnityEngine;

public class FollowController : MonoBehaviour
{
    public float forwardOffset = 0.2f;
    public float rotationOffset = 180;
    public GameObject target;
    public bool matchXRotation = false;
    public bool matchYRotation = true;
    public bool matchZRotation = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            //Match position
            Vector3 newPos = target.transform.position;
            newPos.y = gameObject.transform.position.y;
            newPos.z -= forwardOffset;
            gameObject.transform.position = newPos;


            //Match rotation
            Vector3 newRot = target.transform.eulerAngles;
            if (!matchXRotation) newRot.x = gameObject.transform.eulerAngles.x;
            if (!matchYRotation) newRot.y = gameObject.transform.eulerAngles.y;
            if (!matchZRotation) newRot.z = gameObject.transform.eulerAngles.z;
            newRot.y -= 180;
            gameObject.transform.eulerAngles = newRot;
            //gameObject.transform.rotation = target.transform.rotation;
        }
    }
}
