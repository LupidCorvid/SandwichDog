using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Food : ObjClass
{
    // === SPREADS === //
    private GameObject topSpread;
    private GameObject bottomSpread;

    // === COOKABILITY === //
    [SerializeField] protected GameObject smokeVFX;

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
    [SerializeField] private bool isStackBase;
    [SerializeField] private SandwichBase stackBase;

    [SerializeField] public Transform topPoint; // normal transform should act as bottom

    [SerializeField] public Food debugFoodToSnapTo;

    // === SLICEABILITY === //
    // making slices
    [SerializeField] protected bool isSliceable;
    [SerializeField] protected int numCutsNeeded;
    protected int numCutsMade;
    [SerializeField] GameObject slicedResultObject;

    public bool IsSliceable => isSliceable;

    // results of sliues
    [SerializeField] protected bool wasSliced;
    [SerializeField] Food sliceSource;

    public Food SliceSource => sliceSource;

    public Food(string inObjName = "", bool canCook = false) : base(ObjType.PICKUP, inObjName)
    {
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
    public virtual FoodRequirement AttemptRemoveFoodFromRecipe(List<FoodRequirement> recipeFoods)
    {
        for (int i = 0; i < recipeFoods.Count; i++)
        {
            if (recipeFoods[i].food == this)
            {
                FoodRequirement reqToReturn = recipeFoods[i];
                recipeFoods.RemoveAt(i);
                return reqToReturn;
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

    public override void ApplySpreadVisual(Transform source)
    {
        if (!currentSpreadData.spreadObject) return;

        float sourceDistToBottom = (this.transform.position - source.position).sqrMagnitude;
        float sourceDistToTop = (this.topPoint.position - source.position).sqrMagnitude;

        if (sourceDistToBottom < sourceDistToTop)
        {
            bottomSpread = Instantiate(currentSpreadData.spreadObject, this.transform);
            bottomSpread.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            if (this.isStackBase) stackBase.topStackCollider.enabled = false;
        }
        else
        {
            topSpread = Instantiate(currentSpreadData.spreadObject, this.topPoint);
            if (this.isStackBase) stackBase.topStackCollider.enabled = true;
        }
    }

    public virtual void Cook(float timePassed)
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
