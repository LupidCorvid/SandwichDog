using JetBrains.Annotations;
using UnityEngine;

public class Food : ObjClass
{
    [SerializeField] protected bool isCookable;
    [SerializeField] protected float timeToCook;
    [SerializeField] protected float timeToBurn;
    [SerializeField] protected float cookAmount;

    public bool IsCookable => isCookable;
    public float TimeToCook => timeToCook;
    public float TimeToBurn => timeToBurn;
    public float CookAmount => cookAmount;


    [SerializeField] Color cookedColor;
    private Color burntColor = Color.black;

    public Food(string inObjName = "", bool canCook = false) : base(ObjType.PICKUP, inObjName)
    {
        currentSpread = Spread.NOSPREAD;

        isCookable = canCook;
    }

    protected new void Awake()
    {
        base.Awake();

        if (isCookable)
        {
            cookAmount = 0.0f;
        }
        else
        {
            cookAmount = 1.0f;
        }
    }

    //Compares all member variables of this object and the argument for equality and returns true if all of them match
    public bool compareMemberVars(Food rhs)
    {
        if (this.currentSpread != rhs.currentSpread) return false;
        return true;
    }

    public void Cook(float timePassed)
    {
        cookAmount += timePassed;

        if (cookAmount < timeToCook)
        {
            Color cookColor = Color.Lerp(cleanColor, cookedColor, (cookAmount / TimeToCook));
            objRenderer.material.SetColor("_BaseColor", cookColor);
        }
        else if (cookAmount < (timeToCook + timeToBurn))
        {
            Color burnColor = Color.Lerp(cookedColor, burntColor, ((cookAmount-timeToCook) / (timeToBurn)));
            objRenderer.material.SetColor("_BaseColor", burnColor);
        }
    }

}
