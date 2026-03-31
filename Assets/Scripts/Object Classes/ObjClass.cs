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
    protected const float MAX_CONDITION = 1.0f;

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
    public enum ObjType
    { 
        GRABBABLE, 
        PICKUP, 
        INTERACTABLE 
    }

    public enum Spread
    {
        NOSPREAD,
        PEANUTBUTTER,
        JELLY
    }

    // === INTERACTION === //
    public bool inHand { get; protected set; }
    public bool inMouth { get; protected set; }
    private float grabBuffer = .1f; //Coyote time for 2 handed grab

    // === BASIC INFO === //
    public ObjType objType { get; protected set; }
    public string objName {  get; protected set; }

    // === SCORING === //
    // Used for calculating score
    protected float objCondition;

    public bool canGetDirty;
    public bool canGetClean;

    [SerializeField] private float amountToDirtyPerSecond;
    [SerializeField] private float amountToCleanPerSecond;

    // === VISUALS === //
    private Renderer rendererComponent;

    [SerializeField] private Color objDirtyColor;
    private Color objCleanColor;
    private Vector3 colorDifference;

    private void Awake()
    {
        inHand = false;
        inMouth = false;

        objCondition = MAX_CONDITION;

        InitializeCleanColor();
    }

    //Object manager or subclass should pass in the object type
    public ObjClass(ObjType inObjType, string inObjName = "") 
    {
        inHand = false;
        objType = inObjType;
        objName = inObjName;
        objCondition = 1.0f;
    }

    public override bool Equals(object other)
    {
        ObjClass otherObj = other as ObjClass;

        if (otherObj)
        {
            return this.objName == otherObj.objName && 
                this.objType == otherObj.objType;
        }
        return false;
    }

    private void InitializeCleanColor()
    {
        rendererComponent = GetComponent<Renderer>();

        objCleanColor = rendererComponent.material.GetColor("_BaseColor");

        colorDifference = new Vector3(
            objDirtyColor.r - objCleanColor.r,
            objDirtyColor.g - objCleanColor.g,
            objDirtyColor.b - objCleanColor.b
            );
    }

    //Debug
    //Only allows pickups to be picked up with two hands. Constrains rigidbody if it detects that only one hand is picking it up
    public void CheckTwoHanded()
    {
        //Wait for the player to get a chance to pick the item up with both hands before doing checks
        if (objType == ObjType.PICKUP) StartCoroutine(BufferGrab()); 
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
        if (!inHand && objType == ObjType.GRABBABLE)
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

    public virtual void ReduceObjectCondition()
    {
        float amountToReduce = Time.deltaTime * amountToDirtyPerSecond;

        objCondition = Mathf.Clamp(objCondition - amountToReduce, 0.0f, 1.0f);

        float fraction_dirtied = 1.0f - objCondition;

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
