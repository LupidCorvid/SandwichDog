[System.Serializable]
public class RecipeRequirement
{
    public Food food;



    public int quantity;

    public RecipeRequirement(PlatedFood inPlatedFood, int inQuantity)
    {
        //PlatedFood = inPlatedFood;
        quantity = inQuantity;
    }

    public override bool Equals(object other)
    {
        PlatedFood otherPlatedFood = other as PlatedFood;

        if (otherPlatedFood)
        {
            //PlatedFoodRequirement otherReq = new PlatedFoodRequirement(otherPlatedFood, 1);

            //return PlatedFood == otherReq.PlatedFood && quantity == otherReq.quantity;
        }
        return false;
    }
}