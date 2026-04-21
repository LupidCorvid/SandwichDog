using UnityEngine;

public class hoverAnimScrips : MonoBehaviour
{
    public float downAmount;
    private Vector3 downVector;
    private Vector3 upVector;
    private float timer = 0f;
    private bool state = false;

    void Start()
    {
        downVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - downAmount, gameObject.transform.position.z);
        upVector = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > .65)
        {
            timer = 0;
            state = !state;
        }

        switch (state) 
        { 
            case false:
                gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, downVector, timer / 2);
                break;
            case true:
                gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, upVector, timer / 2);
                break;
        }
    }
}
