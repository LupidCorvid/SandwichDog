using UnityEngine;

public class CookwareCollider : ContainerCollider<Food>
{
    public void CookFood(float amountToCook)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Cook(amountToCook);

            if (ShouldStopCooking(items[i]))
            {
                items.Remove(items[i]);
            }
        }
    }

    private bool ShouldStopCooking(Food food)
    {
        return food.IsBurnt;
    }

    protected override bool CanAddItem(Food food)
    {
        return food.IsCookable && food.CanBeFurtherCooked ? true : false;
    }
}
