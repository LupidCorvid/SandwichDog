using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

// You must derive the base class form monobehavior to attach it to objects in the scene
public class ObjClass : MonoBehaviour
{
    public enum ObjType { GRABBABLE, PICKUP, INTERACTABLE };
    public enum Spread { NOSPREAD, PEANUTBUTTER, JELLY}

    public bool inHand;
    public bool inMouth;
    private float grabBuffer = .1f; //Coyote time for 2 handed grab
    public ObjType m_type;
    public Vector3 startingPosition = new Vector3 (0, 0, 0);

    //Object manager or subclass should pass in the object type
    public ObjClass(ObjType t) 
    {
        inHand = false;
        m_type = t;
    }

    //Debug
    //Only allows pickups to be picked up with two hands. Constrains rigidbody if it detects that only one hand is picking it up
    public void CheckTwoHanded()
    {
        //Wait for the player to get a chance to pick the item up with both hands before doing checks
        if (m_type == ObjType.PICKUP) StartCoroutine(BufferGrab()); 
    }
    IEnumerator BufferGrab()
    {
        yield return new WaitForSeconds(grabBuffer);
        XRGrabInteractable xrgi = gameObject.GetComponent<XRGrabInteractable>();
        if (xrgi.interactorsSelecting.Count == 1) xrgi.interactionLayers = 0;
        if (xrgi.interactorsSelecting.Count == 2) xrgi.interactionLayers = 3;
    }

    //Debug
    public void ReleaseCostraints()
    {
        XRGrabInteractable xrgi = gameObject.GetComponent<XRGrabInteractable>();
        if (xrgi.interactorsSelecting.Count == 1) xrgi.interactionLayers = 0;
        else xrgi.interactionLayers = 3;
    }

    //TODO: Is it more fun to be able to hotswap hand and mouth? Currently set up so that hotswap is a thing
    //TODO: Can these be called by the xr manager?
    public void PutInHand() {
        if (!inHand && m_type == ObjType.GRABBABLE)
        {
            inHand = true;
            if (inMouth) inMouth = false;
        }
    }
    public void DropFromHand() { 
        if(inHand) inHand = false;  
    }
    public void PutInMouth() { 
        if(!inMouth)
        {
            inMouth = true;
            if (inHand) inHand = false;
        }
    }
    public void DropFromMouth() { }
    
}
