using TMPro;
using UnityEngine;

public class MatchRotation : MonoBehaviour
{
    public bool matchXRotation = false;
    public bool matchYRotation = true;
    public bool matchZRotation = false;
    public GameObject target;
    public float forwardOffset;


    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            gameObject.transform.position = target.transform.position + (target.transform.forward * forwardOffset);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);

            Vector3 newRot = target.transform.eulerAngles;

            //Locks rotation for specified axes
            if (!matchXRotation) newRot.x = gameObject.transform.eulerAngles.x;
            if (!matchYRotation) newRot.y = gameObject.transform.eulerAngles.y;
            if (!matchZRotation) newRot.z = gameObject.transform.eulerAngles.z;

            //Model always faces wrong way, this combats it
            //newRot.y -= 180;

            //Apply rotation
            gameObject.transform.eulerAngles = newRot;
        }
    }
}
