using System.Collections;
using UnityEngine;

class CookCollider : MonoBehaviour
{
    private const float MAX_HEAT = 1.0f;

    // cook workers for each class
    [SerializeField] private CookwareCooker indirectCooker;
    [SerializeField] private FoodCooker directCooker;
    //[SerializeField] private float timeToHeatUp;
    //[SerializeField] private float timeToCoolDown;
    private float heatLevel = 1.0f;
    //private bool heatUp;
    //private bool coolDown;

    private void Awake()
    {
        directCooker.heatLevel = this.heatLevel;
        indirectCooker.heatLevel = this.heatLevel;
    }

    public void UpdateCookerHeat()
    {
        directCooker.heatLevel = heatLevel;
        indirectCooker.heatLevel = heatLevel;
    }
}

public class CookwareCooker : TimerCollider<Cookware>
{
    public float cookMultiplier = 1.0f;
    public float heatLevel;

    protected override void TickTimer(Cookware cookware, float timePassed)
    {
        cookware.CookFood(timePassed * heatLevel * cookMultiplier);
    }
}

public class FoodCooker : TimerCollider<Food>
{
    public float cookMultiplier = 1.0f;
    public float heatLevel;

    protected override void TickTimer(Food food, float timePassed)
    {
        food.Cook(timePassed * heatLevel * cookMultiplier);
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
