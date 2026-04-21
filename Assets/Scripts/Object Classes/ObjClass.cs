using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public enum ObjType
{
    GRABBABLE,
    PICKUP,
    INTERACTABLE
}

public enum Spread
{
    NO_SPREAD,
    PEANUT_BUTTER,
    JELLY
}

// You must derive the base class form monobehavior to attach it to objects in the scene
public class ObjClass : MonoBehaviour
{
    protected const float MAX_CONDITION = 1.0f;

    // === INTERACTION=== //
    [SerializeField] protected XRGrabInteractable xrgi;
    [SerializeField] protected XRGeneralGrabTransformer xrggt;
    [SerializeField] protected XRInteractableAffordanceStateProvider xriasp;

    [SerializeField] protected Rigidbody rigidBody;
    public Rigidbody RigidBody => rigidBody;
    private float rbMass; // saved to properly recreate rb
    private List<Collider> xrgiColliders;
    private int xrgiCollidersHash;

    // === BASIC INFO === //
    [SerializeField]  public ObjType objType { get; private set; }
    [SerializeField]  public string objName {  get; private set; }
    [SerializeField] public GlobalObjSettings_SO objSettings;
    [SerializeField] private bool overrideGlobalSettings;

    [SerializeField] public bool triggersTutorial {  get; private set; }

    // === CLEANLINESS === //
    [HideInInspector] public float objCleanliness { get; private set; }
    [SerializeField] protected bool canGetDirty;
    public bool CanGetDirty => canGetDirty;
    [SerializeField] protected bool canGetClean;
    public bool CanGetClean => canGetClean;

    [SerializeField] protected float amountToDirtyPerSecond;
    [SerializeField] protected float amountToCleanPerSecond;

    private Color overloadedDirtColor = new Color32(255, 110, 0, 0);

    // === INTERACTION === //
    public bool inHand { get; private set; }
    public bool inMouth { get; private set; }
    private float grabBuffer = .1f; //Coyote time for 2 handed grab

    // === VISUALS === //
    public Renderer objRenderer { get; private set; }
    protected Color cleanColor;
    [SerializeField] protected Material dirtMaterial;
    private int dirtMatIndex;

    // === SPREADS === //
    [SerializeField] protected bool canHaveSpreads;
    public Spread currentSpread { get; protected set; }
    [SerializeField] protected ObjSpreads_SO possibleSpreads;

    public bool CanHaveSpreads => canHaveSpreads;
    public bool HasSpread => (currentSpread != Spread.NO_SPREAD);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!objRenderer) objRenderer = GetComponent<Renderer>();
        if (!rigidBody) rigidBody = GetComponentInChildren<Rigidbody>();
        if (!xrgi) xrgi = GetComponentInChildren<XRGrabInteractable>();
        if (!xrggt) xrggt = GetComponentInChildren<XRGeneralGrabTransformer>();
        if (!xriasp) xriasp = GetComponentInChildren<XRInteractableAffordanceStateProvider>();
    }
#endif

    protected void Awake()
    {
        inHand = false;
        inMouth = false;

        if (xrgi) xrgi.selectEntered.AddListener(HandlePlayerObjSelectionEntered);
        if (xrgi) xrgi.selectExited.AddListener(HandlePlayerObjSelectionExited);

        objCleanliness = MAX_CONDITION;

        if (objRenderer)
        {
            InitializeAppearanceLogic();
        }

        if (!overrideGlobalSettings)
        {
            InitializeSettings();
        }
    }

    private void HandlePlayerObjSelectionEntered(SelectEnterEventArgs args)
    {
        Player.Instance.activeInteractedObjects.Add(this);
    }

    private void HandlePlayerObjSelectionExited(SelectExitEventArgs args)
    {
        Player.Instance.activeInteractedObjects.Remove(this);
    }

    public virtual void InteractedObjectUpdate()
    {

    }

    protected void Start()
    {
        // TODO - reenable this when tutorial system is refactored
        //if (TutorialManager.Instance.tutorialActive)
        //{
        //    if (this.triggersTutorial)
        //    {
        //        TutorialManager.Instance.AddTutorialItem(this);
        //    }
        //}
    }

    //Object manager or subclass should pass in the object type
    public ObjClass(ObjType inObjType, string inObjName = "")
    {
        inHand = false;
        objType = inObjType;
        objName = inObjName;
        objCleanliness = 1.0f;;
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

    public virtual void InitializeSettings()
    {
        canGetClean = objSettings.canGetDirty;
        dirtMaterial = objSettings.dirtMaterial;
        amountToDirtyPerSecond = objSettings.amountToDirtyPerSecond;

        canGetClean = objSettings.canGetClean;
        amountToCleanPerSecond = objSettings.amountToCleanPerSecond; 
    }

    ///=============================================================================
    ///                             INTERACTION
    ///=============================================================================

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


    public void TransferAndDisableRigidBodiesTo(ObjClass incomingObj=null)
    {
        // record colliders before destruction
        if (xrgi.colliders != null)
        {
            if (xrgiColliders == null || !xrgi.colliders.SequenceEqual(xrgiColliders))
            {
                xrgiColliders = new List<Collider>(xrgi.colliders);
            }
        }
        if (incomingObj)
        {
            foreach (Collider collider in xrgiColliders)
            {
                incomingObj.xrgi.colliders.Add(collider);
            }
        }

        xriasp.enabled = false;
        xriasp.interactableSource = null;

        Destroy(xrgi);

        rbMass = rigidBody.mass;
        Destroy(rigidBody);
    }
    public void EnableRigidBody()
    {
        rigidBody = this.AddComponent<Rigidbody>();
        rigidBody.mass = rbMass;
        xrgi = this.AddComponent<XRGrabInteractable>();
        // restore colliders
        if (xrgiColliders != null)
        {
            foreach (Collider collider in xrgiColliders)
            {
                xrgi.colliders.Add(collider);
            }
        }
    }

    public void DisableInteractability()
    {
        xrgi.selectEntered.RemoveListener(HandlePlayerObjSelectionEntered);
        xrgi.selectExited.RemoveListener(HandlePlayerObjSelectionExited);
        xrgi.enabled = true;

        xrggt.enabled = true;
    }
    public void EnableInteractability()
    {
        xrgi.selectEntered.AddListener(HandlePlayerObjSelectionEntered);
        xrgi.selectExited.AddListener(HandlePlayerObjSelectionExited);
        xrgi.enabled = true;
        xrggt.enabled = true;
    }

    ///=============================================================================
    ///                             APPEARANCE
    ///=============================================================================

    protected virtual void InitializeAppearanceLogic()
    {
        cleanColor = objRenderer != null ? objRenderer.material.GetColor("_BaseColor") : Color.white; //cleanColor = objRenderer?.material?.GetColor("_BaseColor") ?? Color.red;

        if (currentSpread != Spread.NO_SPREAD)
        {
            AddSpread(currentSpread);
        }
        if (canGetDirty)
        {
            AddDirtMaterial();
        }
    }

    //===================
    //       DIRT
    // ==================

    public virtual void DirtyObject(float timeDirtied)
    {
        objCleanliness = Mathf.Clamp(objCleanliness - (timeDirtied * amountToDirtyPerSecond), 0.0f, 1.0f);

        Color dirtColor = objRenderer.material.GetColor("_BaseColor");
        if (overloadedDirtColor != null) dirtColor = overloadedDirtColor;

        dirtColor.a = (MAX_CONDITION - objCleanliness);
        objRenderer.materials[dirtMatIndex].color = dirtColor;
    }

    public virtual void CleanObject(float timeCleaned, float cleanCap = 1.0f)
    {
        // ensure clamp max isn't above max condition
        float maxObjCondition = cleanCap > MAX_CONDITION ? MAX_CONDITION : cleanCap;

        objCleanliness = Mathf.Clamp(objCleanliness - (timeCleaned * amountToCleanPerSecond), 0.0f, maxObjCondition);
    }

    public void AddDirtMaterial()
    {
        List<Material> materials = objRenderer.materials.ToList();
        dirtMatIndex = materials.Count;
        materials.Add(dirtMaterial);
        objRenderer.materials = materials.ToArray();
    }

    //===================
    //       SPREADS
    // ==================

    /// <summary>
    /// Adds new material that adds a material for the incoming spread and removes the previous one. 
    /// </summary>
    /// <returns>Returns if the add was successful or not</returns>
    public bool AddSpread(Spread spreadToAdd)
    {
        if (spreadToAdd == Spread.NO_SPREAD)
        {
            RemoveSpreads();
            return true;
        }

        Material spreadMaterial;
        if (possibleSpreads.GetSpread(spreadToAdd, out spreadMaterial))
        {
            RemoveSpreads();
            objRenderer.AddMaterial(spreadMaterial);
            //Debug.Log(spreadMaterial);
            return true;
        }
        return false;
    }

    public bool RemoveSpreads()
    {
        Material spreadMaterial;
        if (possibleSpreads.GetSpread(currentSpread, out spreadMaterial))
        {
            List<Material> materials = objRenderer.materials.ToList();
            materials.Remove(spreadMaterial);
            objRenderer.materials = materials.ToArray();
            return true;
        }
        return false;
    }
}
