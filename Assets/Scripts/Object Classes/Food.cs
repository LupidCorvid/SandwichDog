using JetBrains.Annotations;
using UnityEngine;

public class Food : ObjClass
{
    // === COOKABILITY === //
    [SerializeField] protected bool isCookable;
    [SerializeField] protected bool isCooked;
    [SerializeField] protected bool isBurnt;
    [SerializeField] protected float timeToCook;
    [SerializeField] protected float timeToBurn;
    [SerializeField] protected float cookAmount;

    // public getter properties
    public bool IsCookable => isCookable;
    public bool IsBurnt => isBurnt;
    public bool IsCooked => IsCooked;

    public float TimeToCook => timeToCook;
    public float TimeToBurn => timeToBurn;
    public float CookAmount => cookAmount;
    // quick reference equation
    public bool CanBeFurtherCooked => (cookAmount < (timeToCook + timeToBurn));

    [SerializeField] Color cookedColor;
    private Color burntColor = Color.black;

    // === SLICEABILITY === //
    [SerializeField] protected bool isSliceable;
    [SerializeField] protected int numCutsNeeded;
    protected int numCutsMade;
    [SerializeField] GameObject slicedResultObject;

    public bool IsSliceable => isSliceable;

    public Food(string inObjName = "", bool canCook = false) : base(ObjType.PICKUP, inObjName)
    {
        currentSpread = Spread.NOSPREAD;

        isCookable = canCook;
    }

    protected new void Awake()
    {
        base.Awake();
        numCutsMade = 0;

        if (isCookable)
        {
            cookAmount = 0.0f;
        }
        else
        {
            cookAmount = 1.0f;
        }
    }

    //Compares all member variables of this object and the argument for equality and returns true if all of them match
    public bool compareMemberVars(Food rhs)
    {
        if (this.currentSpread != rhs.currentSpread) return false;
        return true;
    }

    public void Cook(float timePassed)
    {
        cookAmount += timePassed;

        if (cookAmount < timeToCook)
        {
            Color cookColor = Color.Lerp(cleanColor, cookedColor, (cookAmount / TimeToCook));
            objRenderer.material.SetColor("_BaseColor", cookColor);
            isCooked = true;
        }
        else if (cookAmount > timeToCook && cookAmount <= (timeToCook + timeToBurn))
        {
            Color burnColor = Color.Lerp(cookedColor, burntColor, ((cookAmount-timeToCook) / (timeToBurn)));
            objRenderer.material.SetColor("_BaseColor", burnColor);
        }
        else
        {
            cookAmount = 1.0f;
            objRenderer.material.SetColor("_BaseColor", burntColor);
            isBurnt = true;
        }
    }

    public void Slice()
    {
        numCutsMade += 1;

        if (numCutsMade >= numCutsNeeded)
        {
            Transform thisObjPos = this.transform;

            // hide the obj being cut before destroying it
            objRenderer.enabled = false;
            // TODO - add collision disabling here? might require a rework of prefabs to have a mandated separate collision GO to reference + disable bc there many be many colliders on one obj
            // might be needed bc otherwise some collision logic might be computed + affect things in the next frame

            if (!thisObjPos)
            {
                Debug.LogError(this.name + " is missing a GameObject to replace it when sliced");
                return;
            }
            Instantiate(slicedResultObject, thisObjPos);
            Destroy(this.gameObject);
        }
    }
}
