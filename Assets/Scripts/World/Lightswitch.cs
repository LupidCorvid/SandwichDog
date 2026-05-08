using UnityEngine;

public class Lightswitch : MonoBehaviour
{
    public GameObject light;
    private float snapAngle = 40;


    private void OnTriggerEnter(Collider col)
    {
        if(gameObject.GetComponent<Rigidbody>().angularVelocity.magnitude > 0)
        {
            //Turn off lights
            if(col.name == "OffArea")
            {
                light.SetActive(false);
                //gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, -snapAngle);
            }

            //Turn on lights
            if (col.name == "OnArea")
            {
                light.SetActive(true);
                //gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y, snapAngle);
            }
        }
    }
}
