using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public ObjClass highlighted;
    public bool standing;

    private float standScale = 1f;
    private float crouchScale = .5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlighted = null;
        standing = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (action)
        //{
        //    if (standing) Crouch();
        //    else Stand();
        //}
    }

    public void ToggleStand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (standing)
            {
                standing = false;

                //Set camera
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, crouchScale, gameObject.transform.localScale.z);
            }
            else
            {
                standing = true;

                //Set camera
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, standScale, gameObject.transform.localScale.z);
            }
        }
    }
}
