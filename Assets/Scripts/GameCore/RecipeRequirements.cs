using System;
using UnityEngine;

// ABANDONED FOR URCAD
//[Serializable]
//public class PlatedFood : Food
//{
//    public List<Food> foodOnPlate;
//    public China plate;

//    public override void InteractedObjectUpdate()
//    {
//        //if (plate.transform.eulerAngles)
//    }
//}


[Serializable]
public class FoodRequirement
{
    public Food food;
    public Spread spread;
    public bool isCooked;

    public override bool Equals(object other)
    {
        FoodRequirement otherReq = other as FoodRequirement;

        if (otherReq != null)
        {
            return this.food.Equals(otherReq.food) && this.spread == otherReq.spread && this.isCooked == otherReq.isCooked;
        }
        Food otherFood = other as Food;
        if (otherFood)
        {
            return this.food.Equals(otherFood) && this.spread == otherFood.CurrentSpread;
        }

        return false;
    }
}