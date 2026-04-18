using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

class CookCollider : MonoBehaviour
{
    private const float MAX_HEAT = 1.0f;

    // cook workers for each class
    private CookwareCooker indirectCooker; // food in cookware
    private FoodCooker directCooker; // food directly on burners
    //[SerializeField] private float timeToHeatUp;
    //[SerializeField] private float timeToCoolDown;
    private float heatLevel = 1.0f;
    [SerializeField] private float directSearAmount;
    //private bool heatUp;
    //private bool coolDown;

    private void Awake()
    {
        indirectCooker = this.AddComponent<CookwareCooker>();
        directCooker = this.AddComponent<FoodCooker>();

        directCooker.heatLevel = this.heatLevel * directSearAmount;
        indirectCooker.heatLevel = this.heatLevel;
    }

    public void UpdateCookerHeat()
    {
        directCooker.heatLevel = heatLevel * directSearAmount;
        indirectCooker.heatLevel = heatLevel;
    }
}

public class CookwareCooker : TimerCollider<CookwareCollider>
{
    public float cookMultiplier = 1.0f;
    public float heatLevel;

    protected override void TickTimer(CookwareCollider cookware, float timePassed)
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
        return food.IsBurnt;
    }

    protected override bool CanAddTimer(Food food)
    {
        return food.IsCookable ? true : false;
    }
}
