using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    // === STACKABILITY === //
    [SerializeField] public bool isStackable;

    [SerializeField][HideInInspector] public Transform topStackSnapPoint; // normal transform should act as bottom

    [SerializeField] public Food debugFoodToSnapTo;

    // === SLICEABILITY === //
    [SerializeField] protected bool isSliceable;
    [SerializeField] protected int numCutsNeeded;
    protected int numCutsMade;
    [SerializeField] GameObject slicedResultObject;

    public bool IsSliceable => isSliceable;

    public Food(string inObjName = "", bool canCook = false) : base(ObjType.PICKUP, inObjName)
    {
        currentSpread = Spread.NO_SPREAD;

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
            Color burnColor = Color.Lerp(cookedColor, burntColor, ((cookAmount - timeToCook) / (timeToBurn)));
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

            if (!thisObjPos)
            {
                Debug.LogError(this.name + " is missing a GameObject to replace it when sliced");
                return;
            }
            GameplayManager.Instance.SwapOutObj(this.gameObject, slicedResultObject);
        }
    }
}

///=============================================================================
///                             FOOD ORGANIZATION TYPES
///=============================================================================

[Serializable]
public class PlatedFood : Food
{
    public List<Food> foodOnPlate;
    public China plate;

    public override void InteractedObjectUpdate()
    {
        //if (plate.transform.eulerAngles)
    }
}