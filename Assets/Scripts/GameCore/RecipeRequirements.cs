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

using System;

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
            return this.food == otherReq.food;
        }
        Food otherFood = other as Food;
        if (otherFood)
        {
            return this.food == otherFood;
        }

        return false;
    }
}