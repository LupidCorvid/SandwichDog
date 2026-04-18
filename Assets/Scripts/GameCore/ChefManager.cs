using System;
using System.Collections.Generic;
using UnityEngine;

public class ChefManager : Singleton<ChefManager>
{
    //public List<SandwichBase> potentialSandwiches = new List<SandwichBase>();
    //public List<Sandwich> sandwiches = new List<Sandwich>();

    public List<Food> snapTargets = new List<Food>();

    public void ConstructNewSandwich(Food sandwichBase, Food firstFilling)
    {
        snapTargets.Remove(sandwichBase);
        snapTargets.Remove(firstFilling);
        // disable physics collision
        sandwichBase.RigidBody.isKinematic = true;
        firstFilling.RigidBody.isKinematic = true;
        // prevent stacking with objs individually
        sandwichBase.isStackable = false;
        firstFilling.isStackable = false;

        GameObject sandwichOwner = new GameObject("NewSandwich");
        Sandwich newSandwich = sandwichOwner.AddComponent<Sandwich>();
        newSandwich.InitializeSandwich(sandwichBase, firstFilling);

        snapTargets.Add(newSandwich.GetTopFood());
    }
}