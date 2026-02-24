using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class test : MonoBehaviour
{
    void LateUpdate()
    {

        gameObject.transform.Translate(.01f, 0, 0);

        //Vector3 rot = new Vector3 (gameObject.transform.rotation.x + .1f, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
        //if (rot.x >= 360) rot.x = 0;
        //gameObject.transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        //Debug.Log(rot);

    }
}
