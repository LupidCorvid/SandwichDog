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
            gameObject.transform.position = target.transform.position;
            //gameObject.transform.rotation = target.transform.rotation;
        }
    }
}
