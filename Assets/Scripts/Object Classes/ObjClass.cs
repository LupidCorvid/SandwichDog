using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class FoodRequirement
{
    
}

[System.Serializable]
public class ItemRequirement
{
    public ObjClass item;
    public ObjClass.Spread spread;
    public int quantity;

    public ItemRequirement(ObjClass inItem, int inQuantity)
    {
        item = inItem;
        quantity = inQuantity;
    }

    public override bool Equals(object other)
    {
        ObjClass otherObj = other as ObjClass;

        if (otherObj)
        {
            ItemRequirement otherReq = new ItemRequirement(otherObj, 1);

            return item == otherReq.item && quantity == otherReq.quantity;
        }
        return false;
    }
}

// You must derive the base class form monobehavior to attach it to objects in the scene
public class ObjClass : MonoBehaviour
{
    public enum ObjType { GRABBABLE, PICKUP, INTERACTABLE };
    public enum Spread { NOSPREAD, PEANUTBUTTER, JELLY}

    public bool inHand;
    public bool inMouth;
    private float grabBuffer = .1f; //Coyote time for 2 handed grab

    public ObjType m_type;
    public string m_name;

    public Color objDirtyColor;
    private Color objCleanColor;
    private Vector3 colorDifference;

    private Renderer rendererComponent;

    // Used for calculating score
    public float m_condition { get; private set; }

    private void Awake()
    {
        inHand = false;
        inMouth = false;
        m_condition = 1.0f;
        rendererComponent = GetComponent<Renderer>();
        objCleanColor = rendererComponent.material.GetColor("_BaseColor");
        colorDifference = new Vector3(
            objDirtyColor.r - objCleanColor.r,
            objDirtyColor.g - objCleanColor.g,
            objDirtyColor.b - objCleanColor.b
            );
    }

    //Object manager or subclass should pass in the object type
    public ObjClass(ObjType t, string n = "") 
    {
        inHand = false;
        m_type = t;
        m_name = n;
        m_condition = 1.0f;
    }

    public override bool Equals(object other)
    {
        ObjClass otherObj = other as ObjClass;

        if (otherObj)
        {
            return this.m_name == otherObj.m_name && 
                this.m_type == otherObj.m_type;
        }
        return false;
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

    public void ReduceObjectCondition(float amountToReduce)
    {
        m_condition = Mathf.Clamp(m_condition - amountToReduce, 0.0f, 1.0f);

        float fraction_dirtied = 1.0f - m_condition;

        // max condition & Color RGB value is 1.0f
        Color dirtyColor = new Color(
            objCleanColor.r + (fraction_dirtied * colorDifference.x),
            objCleanColor.g + (fraction_dirtied * colorDifference.y),
            objCleanColor.b + (fraction_dirtied * colorDifference.z)
           );
        Debug.Log(objCleanColor.r + " " + colorDifference.x);
        Debug.Log(fraction_dirtied + " " + colorDifference.x + " " + objCleanColor.r + (fraction_dirtied * colorDifference.x));

        rendererComponent.material.SetColor("_BaseColor", dirtyColor);
    }
}
