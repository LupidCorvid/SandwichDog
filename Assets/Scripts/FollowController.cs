using UnityEngine;

public class FollowController : MonoBehaviour
{
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 newPos = target.transform.position;
            newPos.y = gameObject.transform.position.y;
            gameObject.transform.position = newPos;
            //gameObject.transform.rotation = target.transform.rotation;
        }
    }
}
