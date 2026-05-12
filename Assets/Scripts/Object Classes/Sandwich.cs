using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using static Unity.VisualScripting.Member;

public class Sandwich : Food
{
    private SandwichBase sandwichBase;
    public List<Food> foodOrder = new List<Food>();
    public FoodStackCollider foodStackCollider;

    public Food TopFood => foodOrder[foodOrder.Count - 1];

    public void InitializeSandwich(SandwichBase inSandwichBase, Food firstFilling, FoodStackCollider stackCollider)
    {
        // assign data
        sandwichBase = inSandwichBase;
        foodStackCollider = stackCollider;

        this.isStackable = true;
        foodWeight = inSandwichBase.BaseFood.FoodWeight + firstFilling.FoodWeight;

        foodOrder.Add(sandwichBase.BaseFood);
        StartCoroutine(SandwichFirstStackPhysicsRoutine(firstFilling));
        
        inSandwichBase.BaseFood.objOwner = this;
        firstFilling.objOwner = this;

        // disable incoming food ticking, as the sandwich now governs it
        sandwichBase.BaseFood.enabled = false;
        firstFilling.enabled = false;
    }

    public void PopTopFood()
    {
        Food topFood = TopFood;
        ReleaseFood(topFood);

        Food newTopFood = TopFood;
        topFood.EnableInteractability();
        if (foodOrder.Count <= 0)
        {
            // FREE THE SANDWICH BASE
            //sandwichBase.
        }
    }
    
    private void ReleaseFood(Food food)
    {
        foodOrder.Remove(food);
        food.EnableRigidBody();
        //food.EnableInteractability();
        // leave sandwich hierarchy
        food.transform.SetParent(null, true); 
        food.objOwner = null;
    }

    public override bool Equals(object other)
    {
        Sandwich otherSandwich = other as Sandwich;
        if (otherSandwich) return Equals(otherSandwich);

        return false;
    }

    private bool Equals(Sandwich otherSandwich)
    {
        if (otherSandwich.foodOrder.Count != foodOrder.Count) return false;

        for (int i = 0; i < foodOrder.Count; i++)
        {
            if (otherSandwich.foodOrder[i] != this.foodOrder[i]) return false;
        }
        return true;
    }

    private IEnumerator SandwichFirstStackPhysicsRoutine(Food targetFood)
    {
        sandwichBase.BaseFood.TransferAndDisableRigidBodiesTo(this);
        targetFood.TransferAndDisableRigidBodiesTo(this);

        foodStackCollider.transform.SetParent(this.transform, true);
        sandwichBase.transform.SetParent(this.transform, true);
        targetFood.transform.SetParent(this.transform, true);

        this.transform.rotation = sandwichBase.transform.rotation;
        sandwichBase.transform.rotation = Quaternion.identity;
        sandwichBase.transform.localPosition = Vector3.zero;
        //foodStackCollider.transform.Translate(new Vector3(
        //    0.0f, 
        //    Mathf.Abs(sandwichBase.BaseFood.topPoint.transform.position.y - sandwichBase.BaseFood.transform.position.y),
        //    0.0f)
        //    , Space.Self);

        sandwichBase.BaseFood.objOwner = this;
        targetFood.objOwner = this;

        SnapToTop(targetFood);
        this.RigidBody.WakeUp();

        yield return new WaitForFixedUpdate();
        this.RigidBody.ResetCenterOfMass();

        foodOrder.Add(targetFood);
    }

    private IEnumerator SandwichNewItemPhysicsRoutine(Food targetFood)
    {
        targetFood.TransferAndDisableRigidBodiesTo(this);
        targetFood.transform.SetParent(this.transform, true);

        targetFood.objOwner = this;

        SnapToTop(targetFood);
        this.RigidBody.WakeUp();
        //SnapToTop(TopFood, targetFood);

        yield return new WaitForFixedUpdate();
        this.RigidBody.ResetCenterOfMass();

        foodOrder.Add(targetFood);
        foodWeight += targetFood.FoodWeight;
    }

    public void SnapToTop(Food target)
    {
        Food source = TopFood;
        bool areBothObjsSameDir = Vector3.Dot(source.transform.up, target.transform.up) > 0.0f ? true : false;

        /*
         * upside down = UD
         * rightside up = RU
         * this food to move = curr
         * base food to snap to = target
        */

        //Debug.Log(source.transform.position + " " + source.topPoint.transform.position);
        //Debug.Log(target.transform.position + " " + target.topPoint.transform.position);

        float oldYRotation = target.transform.rotation.eulerAngles.y;
        target.transform.rotation = Quaternion.Euler(0, oldYRotation, 0);

        if (areBothObjsSameDir)
        {
            AlignWithTop(target.transform, false);
        }
        else
        {
            AlignWithTop(target.transform, true);
        }

        // now calc aligned distances

        float currOriginToTargetTop = (source.transform.position - target.topPoint.position).sqrMagnitude;
        float currOriginToTargetOrigin = (source.transform.position - target.transform.position).sqrMagnitude;
        Vector3 targetHeight = new Vector3(0.0f, Mathf.Abs(target.topPoint.transform.localPosition.y - target.transform.localPosition.y), 0.0f);
        Vector3 sourceHeight = new Vector3(0.0f, Mathf.Abs(source.topPoint.transform.localPosition.y - source.transform.localPosition.y), 0.0f);

        Vector3 distanceToMove = Vector3.zero;

        //target.transform.rotation = Quaternion.identity;

        if (areBothObjsSameDir)
        {
            //Debug.Log("rotation was set as " + target.transform.localEulerAngles);
            // case 1: RU on RU, curr top is closest to target origin
            if (currOriginToTargetTop < currOriginToTargetOrigin)
            {
                //Debug.Log("case 1");
                distanceToMove = targetHeight;
            }
            // case 2: UD on UD, curr origin closest to target top
            else
            {
                //Debug.Log("case 2");
                distanceToMove = sourceHeight;
            }
        }
        // objs facing opposite directions
        else
        {
            //Debug.Log("rotation was set as " + target.transform.localEulerAngles);
            // case 3: RU on UD, current origin is closest to target origin
            if (currOriginToTargetOrigin < currOriginToTargetTop)
            {
                //Debug.Log("case 3");
                distanceToMove = Vector3.zero;
            }
            // case 4: UD on RU, current top is closest to target top
            else
            {
                //Debug.Log("case 4");
                distanceToMove = sourceHeight;
            }
        }
        target.transform.position = source.transform.position + distanceToMove;

        foodStackCollider.transform.Translate(targetHeight, Space.Self);
        //foodStackCollider.transform.Translate(0.0f, Mathf.Abs(targetTopPos.y), 0.0f, Space.Self);
    }

    private void AlignWithTop(Transform target, bool flipZRotation)
    {
        Transform source = TopFood.transform;

        // align curr with target rotation along plane where the snap point lies
        Vector3 flattenedForward = Vector3.ProjectOnPlane(source.transform.forward, target.transform.up);

        // avoid gimbal locking if both curr + target perfectly up
        if (flattenedForward.sqrMagnitude < Mathf.Epsilon)
        {
            flattenedForward = Vector3.ProjectOnPlane(source.transform.up, target.transform.up);
        }
        // apply new rotation
        Quaternion targetRotation = Quaternion.LookRotation(flattenedForward, target.transform.up);

        foodStackCollider.transform.rotation = targetRotation;

        if (flipZRotation)
        {
            targetRotation = Quaternion.Euler(
                targetRotation.eulerAngles.x,
                targetRotation.eulerAngles.y,
                targetRotation.eulerAngles.z + 180.0f
            );
        }
        //Debug.Log("set rotation to " + targetRotation.eulerAngles);

        source.rotation = targetRotation;
    }

    public void AddToSandwich(Food newFood)
    {
        StartCoroutine(SandwichNewItemPhysicsRoutine(newFood));
        AcquireChild(newFood);


        SandwichBase possibleCap = newFood.GetComponent<SandwichBase>();
        // cap the sandwich if another bread is added
        if (possibleCap)
        {
            this.isStackable = false;
            AcquireChild(possibleCap.BaseFood);
            possibleCap.DisableBothTriggers();
            possibleCap.enabled = false;
        }
    }

    public override void AcquireChild(ObjClass newChild)
    {
        base.AcquireChild(newChild); // remove timers on child
        newChild.enabled = false;
    }

    public override void Cook(float timePassed)
    {
        foreach (Food food in foodOrder)
        {
            if (food.IsCookable && food.CanBeFurtherCooked) food.Cook(timePassed);
        }
    }

    public override void DirtyObject(float timePassed)
    {
        foreach (Food food in foodOrder)
        {
            if (food.CanGetDirty) food.DirtyObject(timePassed);
        }
    }

    public override void CleanObject(float timeCleaned, float cleanCap = 1)
    {
        foreach (Food food in foodOrder)
        {
            if (food.CanGetClean) food.CleanObject(timeCleaned);
        }
    }
}
