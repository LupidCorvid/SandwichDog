using System;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : Food
{
    public List<Food> foodOrder = new List<Food>();

    public Transform sandwichBottomAnchor { get; private set; }
    public Transform sandwichTopAnchor { get; private set; }

    public void InitializeSandwich(Food sandwichBase, Food firstFilling)
    {
        this.isStackable = true;
        foodOrder.Add(sandwichBase);

        sandwichBottomAnchor = new GameObject("SandwichBottomAnchor").transform;
        sandwichBottomAnchor.SetParent(this.transform);
        //bottomOfSandwich.transform.position = sandwichBase.transform.position;
        //bottomOfSandwich.transform.rotation = sandwichBase.transform.rotation;
        //bottomOfSandwich.transform.parent = this.transform;

        sandwichTopAnchor = new GameObject("SandwichTopAnchor").transform;
        sandwichTopAnchor.SetParent(this.transform);

        firstFilling.SnapTo(sandwichTopAnchor);
        foodOrder.Add(firstFilling);
    }

    public void AddNewFood(Food newFood)
    {
        newFood.RigidBody.isKinematic = false;
        newFood.isStackable = false;

        newFood.SnapTo(sandwichTopAnchor);
        foodOrder.Add(newFood);
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
}