using UnityEngine;

public class CookwareCollider : ContainerCollider<Food>
{
    public void CookFood(float amountToCook)
    {
        foreach (Food food in items)
        {
            food.Cook(amountToCook);

            if (ShouldStopCooking(food))
            {
                items.Remove(food);
            }
        }
    }

    private bool ShouldStopCooking(Food food)
    {
        return food.CookAmount > (food.TimeToCook + food.TimeToBurn);
    }

    protected override bool CanAddItem(Food food)
    {
        return food.IsCookable && food.CanBeFurtherCooked ? true : false;
    }
}
