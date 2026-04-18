using JetBrains.Annotations;
using System;
using System.Collections.Generic;
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

    [SerializeField] protected static float minDistanceToSnapObj;

    [SerializeField][HideInInspector] public Transform topStackSnapPoint; // normal transform should act as bottom

    [SerializeField][HideInInspector] public Transform foodCenterPoint;

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

    public override void InteractedObjectUpdate()
    {
        base.InteractedObjectUpdate();

        if (isStackable)
        {
            // find closest sandwich base to snap to
            float closestDistance = float.PositiveInfinity;
            Food closestFood = null;

            foreach (Food targetFood in ChefManager.Instance.snapTargets)
            {
                float distanceToSnapTarget = (targetFood.transform.position - this.transform.position).sqrMagnitude;

                if (distanceToSnapTarget < (minDistanceToSnapObj * minDistanceToSnapObj))
                {
                    if (distanceToSnapTarget < closestDistance)
                    {
                        closestDistance = distanceToSnapTarget;
                        closestFood = targetFood;
                    }
                }
            }

            // snap to sandwich base
            if (closestFood != null)
            {
                float distanceFromTop = (closestFood.topStackSnapPoint.position - this.topStackSnapPoint.position).sqrMagnitude;
                bool doReverseSnap = false;

                Vector3 posToSnapTo = Vector3.zero;

                if (closestDistance < distanceFromTop)
                {
                    posToSnapTo = closestFood.transform.position;
                }
                else
                {
                    posToSnapTo = closestFood.topStackSnapPoint.position;
                }

                return;
            }

            // find closest sandwich top to snap to
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

    ///=============================================================================
    ///                             STACKING FOODS
    ///=============================================================================

    // returns new top point
    public void SnapTo(Transform target)
    {
        bool isCurrUpsideDown = Vector3.Dot(Vector3.up, this.transform.up) > 0.0f ? true : false;
        bool isTargetUpsideDown = Vector3.Dot(Vector3.up, target.transform.up) > 0.0f ? true : false;

        /*
         * upside down = UD
         * rightside up = RU
         * this food to move = curr
         * base food to snap to = target
        */

        float minDistNeeded = minDistanceToSnapObj * minDistanceToSnapObj;

        // case 1: RU on RU, curr origin closest to target top
        if (!isCurrUpsideDown && !isTargetUpsideDown)
        {
            if ((this.transform.position - target.position).sqrMagnitude < minDistanceToSnapObj)
            {
                this.AlignWith(target);
            }
        }
        // case 2: UD on UD, curr top is closest to target origin
        else if (isCurrUpsideDown && isTargetUpsideDown)
        {
            if ((this.topStackSnapPoint.position - target.position).sqrMagnitude < minDistanceToSnapObj)
            {
                this.transform.position += (this.transform.position - this.topStackSnapPoint.position);
                this.AlignWith(target);
            }
        }
        // case 3: RU on UD, current origin is closest to target origin
        else if (!isCurrUpsideDown && isTargetUpsideDown)
        {
            if ((this.transform.position - target.transform.position).sqrMagnitude < minDistanceToSnapObj)
            {
                this.AlignWith(target);
            }
        }
        // case 4: UD on RU, current top is closest to target top
        else if (isCurrUpsideDown && !isTargetUpsideDown)
        {
            if ((this.topStackSnapPoint.position - target.position).sqrMagnitude < minDistanceToSnapObj)
            {
                this.transform.position += (this.transform.position - this.topStackSnapPoint.position);
                this.AlignWith(target);
            }
        }
    }

    protected void AlignWith(Transform target)
    {
        this.transform.position = target.position;

        // align curr with target rotation along plane where the snap point lies
        Vector3 flattenedForward = Vector3.ProjectOnPlane(this.transform.forward, target.transform.up);

        // avoid gimbal locking if both curr + target perfectly up
        if (flattenedForward.sqrMagnitude < Mathf.Epsilon)
        {
            flattenedForward = Vector3.ProjectOnPlane(transform.up, target.transform.up);
        }

        // apply new rotation
        Quaternion targetRotation = Quaternion.LookRotation(flattenedForward, target.transform.up);
        this.transform.rotation = targetRotation;
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