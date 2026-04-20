using System;
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
        sandwichBase = inSandwichBase;
        Food baseFood = sandwichBase.BaseFood;
        foodStackCollider = stackCollider;

        this.isStackable = true;
        PushBaseFood(baseFood);
        PushNewFood(firstFilling);
    }

    public void PushBaseFood(Food baseFood)
    {
        baseFood.CopyColliders(this);
        baseFood.gameObject.transform.SetParent(this.transform);
        foodStackCollider.gameObject.transform.SetParent(this.transform);
        // TRANSFER NEW SANDWICH OBJ TO PLAYER
        if (Player.Instance.activeInteractedObjects.Contains(baseFood))
        {

        }
        else
        {
            //baseFood.DisableInteractability();
        }
        foodOrder.Add(baseFood);
        //Debug.Log(baseFood.transform.position.y);
        //Debug.Log("why " + Mathf.Abs(baseFood.topStackSnapPoint.transform.position.y - baseFood.transform.position.y));
        //foodStackCollider.transform.position = new Vector3(
        //    baseFood.transform.position.x, 
        //    baseFood.transform.position.y + Mathf.Abs(baseFood.topStackSnapPoint.transform.position.y - baseFood.transform.position.y), 
        //    baseFood.transform.position.z
        //);
        //foodStackCollider.transform.position = baseFood.transform.position;
        foodStackCollider.transform.localPosition = new Vector3(
            0.0f,
            Mathf.Abs(baseFood.topStackSnapPoint.transform.position.y - baseFood.transform.position.y),
            0.0f
        );
    }

    public void PushNewFood(Food newFood)
    {
        if (foodOrder.Exists(food => ReferenceEquals(food, newFood))) return;

        newFood.CopyColliders(this);
        newFood.gameObject.transform.SetParent(this.transform);

        // force player to release the incoming obj if applicable
        if (Player.Instance.activeInteractedObjects.Contains(newFood))
        {
            //newFood.DisableInteractability();
            Player.Instance.activeInteractedObjects.Remove(newFood);
        }

        Debug.Log("snap to " + GetTopFood().name + "!");

        SnapTo(GetTopFood(), newFood);
        //foodStackCollider.transform.position += Vector3.up;
        foodOrder.Add(newFood);
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

    // returns distance moved
    private void SnapTo(Food source, Food target)
    {
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
        AlignWith(source.transform, target.transform, true);

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
        foodStackCollider.transform.position += currTopPos;
    }

    private void AlignWith(Transform source, Transform target, bool flipZRotation)
    {
        // align curr with target rotation along plane where the snap point lies
        Vector3 flattenedForward = Vector3.ProjectOnPlane(source.transform.forward, target.transform.up);

        // avoid gimbal locking if both curr + target perfectly up
        if (flattenedForward.sqrMagnitude < Mathf.Epsilon)
        {
            flattenedForward = Vector3.ProjectOnPlane(transform.up, target.transform.up);
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
