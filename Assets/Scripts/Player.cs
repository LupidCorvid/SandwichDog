using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using NUnit.Framework;

public class Player : Singleton<Player>
{

    public ObjClass highlighted;
    public GameObject dog;
    public bool standing;

    public List<ObjClass> activeInteractedObjects = new List<ObjClass>();

    private float standScale = 1f;
    private float crouchScale = .85f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highlighted = null;
        standing = true;
    }

    private void FixedUpdate()
    {
        foreach (ObjClass interactedObj in activeInteractedObjects)
        {
            interactedObj.InteractedObjectUpdate();
        }
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

                //Set animation bool
                dog.GetComponent<Animator>().SetBool("standing", false);

                //Set player Y follow offset
                dog.GetComponent<FollowController>().ToggleYOffset(standing);

                //TODO: test after testing y offset
                ToggleHandTracking();
            }
            else
            {
                standing = true;

                //Set camera
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, standScale, gameObject.transform.localScale.z);

                //Set animation bool
                dog.GetComponent<Animator>().SetBool("standing", true);

                //Set player Y follow offset
                dog.GetComponent<FollowController>().ToggleYOffset(standing);

                //TODO: test after testing y offset
                ToggleHandTracking();
            }
        }
    }

    public void ToggleHandTracking()
    {
        dog.GetComponent<RigBuilder>().enabled = !dog.GetComponent<RigBuilder>().enabled;
    }

    public void WalkingStarted(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dog.GetComponent<Animator>().SetBool("walking", true);
        }
        else if (context.canceled)
        {
            dog.GetComponent<Animator>().SetBool("walking", false);
        }
    }
}
