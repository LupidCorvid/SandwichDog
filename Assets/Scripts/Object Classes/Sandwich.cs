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
    public FoodStackCollider foodStackCollider;

    public Food TopFood => foodOrder[foodOrder.Count - 1];

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
        // leave sandwich hierarchy
        food.transform.SetParent(this.transform.parent, true); 
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

    public override Food AttemptRemoveFoodFromRecipe(List<Food> recipeFoods)
    {
        foreach (Food sandwichFood in foodOrder)
        {
            foreach (Food recipeFood in recipeFoods)
            {
                if (sandwichFood == recipeFood)
                {
                    foodOrder.Remove(recipeFood); // bad and destructive of the original data but shouldn't matter at the end of a level
                    return sandwichFood;
                }
            }
        }
        return null;
    }

    public override float GetFoodWeight()
    {
        return foodOrder.Count;
    }

    public override float ScoreFood()
    {
        float scoreSum = 0.0f;
        foreach (Food food in foodOrder)
        {
            scoreSum += food.ScoreFood();
        }
        return (scoreSum / foodOrder.Count);
    }

    private IEnumerator SandwichFirstStackPhysicsRoutine(Food targetFood)
    {
        sandwichBase.BaseFood.TransferAndDisableRigidBodiesTo(this);
        targetFood.TransferAndDisableRigidBodiesTo(this);

        float foodOffset = sandwichBase.BaseFood.topStackSnapPoint.localPosition.y;

        foodStackCollider.transform.SetParent(this.transform, true);
        sandwichBase.BaseFood.transform.SetParent(this.transform, true);
        targetFood.transform.SetParent(this.transform, true);

        foodStackCollider.transform.Translate(0.0f, foodOffset, 0.0f, Space.Self);

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

        //SnapToTop(TopFood, targetFood);

        yield return new WaitForFixedUpdate();
        this.RigidBody.ResetCenterOfMass();

        foodOrder.Add(targetFood);
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

        Debug.Log(source.transform.position + " " + source.topStackSnapPoint.transform.position);
        Debug.Log(target.transform.position + " " + target.topStackSnapPoint.transform.position);

        float currOriginToTargetTop = (source.transform.position - target.topStackSnapPoint.position).sqrMagnitude;
        float currOriginToTargetOrigin = (source.transform.position - target.transform.position).sqrMagnitude;
        Vector3 targetTopPos = new Vector3(0.0f, Mathf.Abs(target.topStackSnapPoint.transform.position.y - target.transform.position.y), 0.0f);
        Vector3 currTopPos = new Vector3(0.0f, Mathf.Abs(source.topStackSnapPoint.transform.position.y - source.transform.position.y), 0.0f);

        Vector3 distanceToMove = Vector3.zero;

        if (areBothObjsSameDir)
        {
            // case 1: RU on RU, curr origin closest to target top
            if (currOriginToTargetTop < currOriginToTargetOrigin)
            {
                Debug.Log("case 1");
                distanceToMove = targetTopPos;
            }
            // case 2: UD on UD, curr top is closest to target origin
            else
            {
                Debug.Log("case 2");
                distanceToMove = currTopPos;
            }
            AlignWithTop(target.transform, false);
        }
        // objs facing opposite directions
        else
        {
            // case 3: RU on UD, current origin is closest to target origin
            if (currOriginToTargetOrigin < currOriginToTargetTop)
            {
                Debug.Log("case 3");
                distanceToMove = Vector3.zero;
            }
            // case 4: UD on RU, current top is closest to target top
            else
            {
                Debug.Log("case 4");
                distanceToMove = targetTopPos + currTopPos;
            }
            AlignWithTop(target.transform, true);
        }
        source.transform.position = target.transform.position + distanceToMove;

        foodStackCollider.transform.Translate(0.0f, targetTopPos.y, 0.0f, Space.Self);
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


        if (flipZRotation)
        {
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z + 180.0f);
        }
        source.transform.rotation = targetRotation;
    }
}
