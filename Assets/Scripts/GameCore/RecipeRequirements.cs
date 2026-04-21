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
public class RecipeRequirement
{
    public ObjClass item;
    public Spread spread;
    public int quantity;

    public RecipeRequirement(ObjClass inItem, int inQuantity)
    {
        item = inItem;
        quantity = inQuantity;
    }

    public override bool Equals(object other)
    {
        ObjClass otherObj = other as ObjClass;

        if (otherObj)
        {
            RecipeRequirement otherReq = new RecipeRequirement(otherObj, 1);

            return item == otherReq.item && quantity == otherReq.quantity;
        }
        return false;
    }
}