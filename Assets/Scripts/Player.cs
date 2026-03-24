using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{

    public ObjClass highlighted;
    public GameObject dog;
    public bool standing;

    private float standScale = 1f;
    private float crouchScale = .5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlighted = null;
        standing = false;
    }

    public void ToggleStand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("Toggled!");
            if (standing)
            {
                standing = false;

                //Set camera
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, crouchScale, gameObject.transform.localScale.z);

                //Set animation
                dog.GetComponent<Animator>().SetBool("standing", false);
            }
            else
            {
                standing = true;
                dog.GetComponent<Animator>().SetBool("standing", true);

                //Set camera
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, standScale, gameObject.transform.localScale.z);

                //Set animation
                dog.GetComponent<Animator>().Play("StandWalk");

            }
        }
    }

    public void WalkingStarted(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dog.GetComponent<Animator>().SetBool("walking", true);
        }
        else if (context.canceled)
        {
            dog.GetComponent<Animator>().SetBool("walking", false);
        }
    }
}
