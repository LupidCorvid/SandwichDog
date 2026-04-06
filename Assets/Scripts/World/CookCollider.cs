using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum CookType
{
    Pan,
    Bake
}

public class CookCollider : TimerHolder<Food>
{
    [SerializeField] private CookType cookType;

    protected override void TickTimer(Food food, float timePassed)
    {
        food.Cook(timePassed);
    }

    protected override bool ShouldRemoveTimer(Food food)
    {
        return food.CookAmount > (food.TimeToCook + food.TimeToBurn);
    }

    protected override bool CanAddTimer(Food food)
    {
        return food.IsCookable ? true : false;
    }
}
