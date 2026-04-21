using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using static Unity.VisualScripting.Member;

public class Sandwich : Food
{
    private SandwichBase sandwichBase;
    public List<Food> foodOrder = new List<Food>();
    private FoodStackCollider foodStackCollider;

    public void InitializeSandwich(SandwichBase inSandwichBase, Food firstFilling, FoodStackCollider stackCollider)
    {
        // assign data
        sandwichBase = inSandwichBase;
        foodStackCollider = stackCollider;

        this.isStackable = true;

        foodOrder.Add(sandwichBase.BaseFood);
        StartCoroutine(SandwichFirstStackPhysicsRoutine(firstFilling));
    }

    public void PushNewFood(Food newFood)
    {
        StartCoroutine(SandwichNewItemPhysicsRoutine(newFood));
    }

    public void PopTopFood()
    {
        Food topFood = GetTopFood();
        ReleaseFood(topFood);

        Food newTopFood = GetTopFood();
        topFood.EnableInteractability();
        if (foodOrder.Count <= 0)
        {
            sandwichBase.HandleDestroySandwich();
        }
    }
    
    private void ReleaseFood(Food food)
    {
        foodOrder.Remove(food);
        food.EnableRigidBody();
        // leave sandwich hierarchy
        food.transform.SetParent(this.transform.parent, true); 
    }

    public override bool Equals(object other)
    {
        Sandwich otherSandwich = other as Sandwich;

        if (!otherSandwich) return false;

        if (otherSandwich.foodOrder.Count != foodOrder.Count) return false;

        for (int i = 0; i < foodOrder.Count; i++)
        {
            if (otherSandwich.foodOrder[i] != this.foodOrder[i]) return false;
        }
        return true;
    }

    public Food GetTopFood() { return foodOrder[foodOrder.Count - 1]; }

    private IEnumerator SandwichFirstStackPhysicsRoutine(Food targetFood)
    {
        sandwichBase.BaseFood.TransferAndDisableRigidBodiesTo(this);
        targetFood.TransferAndDisableRigidBodiesTo(this);

        foodStackCollider.transform.SetParent(this.transform);
        sandwichBase.BaseFood.transform.SetParent(this.transform);
        targetFood.transform.SetParent(this.transform);

        SnapToTop(targetFood);

        this.RigidBody.WakeUp();

        yield return new WaitForFixedUpdate();
        this.RigidBody.ResetCenterOfMass();

        foodOrder.Add(targetFood);
    }

    private IEnumerator SandwichNewItemPhysicsRoutine(Food targetFood)
    {
        targetFood.TransferAndDisableRigidBodiesTo(this);
        targetFood.transform.SetParent(this.transform);

        SnapToTop(targetFood);

        yield return new WaitForFixedUpdate();
        this.RigidBody.ResetCenterOfMass();

        foodOrder.Add(targetFood);
    }

    public void SnapToTop(Food target)
    {
        Food source = GetTopFood();
        bool areBothObjsSameDir = Vector3.Dot(source.transform.up, target.transform.up) > 0.0f ? true : false;

        /*
         * upside down = UD
         * rightside up = RU
         * this food to move = curr
         * base food to snap to = target
        */

        float currOriginToTargetTop = (source.transform.position - target.topStackSnapPoint.position).sqrMagnitude;
        float currOriginToTargetOrigin = (source.transform.position - target.transform.position).sqrMagnitude;
        Vector3 targetTopPos = new Vector3(0.0f, Mathf.Abs(target.topStackSnapPoint.transform.position.y - target.transform.position.y), 0.0f);
        Vector3 currTopPos = new Vector3(0.0f, Mathf.Abs(source.topStackSnapPoint.transform.position.y - source.transform.position.y), 0.0f);

        Vector3 distanceToMove = Vector3.zero;
        AlignWithTop(target.transform, true);

        if (areBothObjsSameDir)
        {
            // case 1: RU on RU, curr origin closest to target top
            if (currOriginToTargetTop < currOriginToTargetOrigin)
            {
                distanceToMove = targetTopPos;
            }
            // case 2: UD on UD, curr top is closest to target origin
            else
            {
                distanceToMove = currTopPos;
            }
        }
        // objs facing opposite directions
        else
        {
            // case 3: RU on UD, current origin is closest to target origin
            if (currOriginToTargetOrigin < currOriginToTargetTop)
            {
                distanceToMove = Vector3.zero;
            }
            // case 4: UD on RU, current top is closest to target top
            else
            {
                distanceToMove = targetTopPos + currTopPos;
            }
        }
        source.transform.position = target.transform.position + distanceToMove;
        //foodStackCollider.transform.position += currTopPos;
    }

    private void AlignWithTop(Transform target, bool flipZRotation)
    {
        Transform source = GetTopFood().transform;

        // align curr with target rotation along plane where the snap point lies
        Vector3 flattenedForward = Vector3.ProjectOnPlane(source.transform.forward, target.transform.up);

        // avoid gimbal locking if both curr + target perfectly up
        if (flattenedForward.sqrMagnitude < Mathf.Epsilon)
        {
            flattenedForward = Vector3.ProjectOnPlane(source.transform.up, target.transform.up);
        }
        // apply new rotation
        Quaternion targetRotation = Quaternion.LookRotation(flattenedForward, target.transform.up);


        if (flipZRotation)
        {
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z + 180.0f);
        }
        source.transform.rotation = targetRotation;
    }
}
