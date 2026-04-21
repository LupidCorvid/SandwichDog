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
    [SerializeField] protected bool isOvercooked;
    [SerializeField] protected bool isBurnt;
    [SerializeField] protected float timeToCook;
    [SerializeField] protected float timeToBurn;
    [SerializeField] protected float cookAmount;
    [SerializeField] protected float overcookAmount;

    // public getter properties
    public bool IsCookable => isCookable;
    public bool IsBurnt => isBurnt;
    public float TimeToCook => timeToCook;
    public float TimeToBurn => timeToBurn;
    public float CookAmount => cookAmount;
    public float OvercookAmount => overcookAmount;
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
    [SerializeField] Food sliceProduct;
    [SerializeField] protected int numCutsNeeded;
    protected int numCutsMade;
    [SerializeField] GameObject slicedResultObject;

    public Food SliceProduct => sliceProduct;

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

    // this is probably scuffed with the null return but it'll do
    public virtual Food AttemptRemoveFoodFromRecipe(List<Food> recipeFoods)
    {
        for (int i = 0; i < recipeFoods.Count; i++)
        {
            if (recipeFoods[i] == this)
            {
                recipeFoods.RemoveAt(i);
                return this;
            }
        }
        return null;
    }

    public virtual float GetFoodWeight()
    {
        return 1.0f;
    }

    public virtual float ScoreFood()
    {
        float scoreSum = 0.0f;
        float factorsScored = 0.0f;

        if (isCookable)
        {
            scoreSum += Mathf.SmoothStep(0.0f, 1.0f, (cookAmount / timeToCook));
            Debug.Log("adding " + Mathf.SmoothStep(0.0f, 1.0f, (cookAmount % timeToCook)) + " from cooking");

            if (isOvercooked)
            {
                scoreSum -= Mathf.SmoothStep(0.0f, 1.0f, overcookAmount);
                Debug.Log("subtracting " + Mathf.SmoothStep(0.0f, 1.0f, overcookAmount) + " from overcooking");
            }
        }
        if (canGetDirty)
        {
            scoreSum += Mathf.SmoothStep(0.0f, 1.0f, objCleanliness);
            Debug.Log("adding " + Mathf.SmoothStep(0.0f, 1.0f, objCleanliness) + " from cleanliness");
        }
        float score = (scoreSum / factorsScored);

        return score;
    }

    public void Cook(float timePassed)
    {
        if (cookAmount < 1.0f)
        {
            cookAmount = Mathf.Clamp((cookAmount + (timePassed / timeToCook)), 0.0f, 1.0f);
            Color cookColor = Color.Lerp(cleanColor, cookedColor, cookAmount);
            objRenderer.material.SetColor("_BaseColor", cookColor);
        }
        else if (overcookAmount < 1.0f)
        {
            isOvercooked = true;
            overcookAmount = Mathf.Clamp((overcookAmount + (timePassed / timeToBurn)), 0.0f, 1.0f);
            Color overcookColor = Color.Lerp(cookedColor, burntColor, overcookAmount);
            objRenderer.material.SetColor("_BaseColor", overcookColor);
        }
        else
        {
            isBurnt = true;
            overcookAmount = 1.0f;
            objRenderer.material.SetColor("_BaseColor", burntColor);
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
