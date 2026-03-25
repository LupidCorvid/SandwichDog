using JetBrains.Annotations;
using UnityEngine;

public enum FoodCondition
{
    Normal, // non-cookable food
    Raw,
    Cooked, 
    Burnt
}

public class Food : ObjClass
{
    public Spread currentSpread;
    public GameObject mesh;
    public Material[] matSpreads;

    public FoodCondition condition;

    public bool isCookable;
    [SerializeField] private float timeToCook;
    [SerializeField] private float timeToBurn;
    public float cookTimeStart;
    public float cookAmount;

    public Food(string n = "") : base(ObjType.PICKUP)
    {
        currentSpread = Spread.NOSPREAD;
        m_name = n;

        isCookable = false;
        cookTimeStart = 0.0f;
        cookAmount = 0.0f;
    }

    public void addSpread(Spread s)
    {
        currentSpread = s;
        if (mesh != null)
        {
            int index = (int)(s);
            mesh.GetComponent<MeshRenderer>().material = matSpreads[index];
        }
    }
    public void removeSpread()
    {
        currentSpread = Spread.NOSPREAD;
        if (mesh != null)
        {
            mesh.GetComponent<MeshRenderer>().material = matSpreads[0];
        }
    }

    //Compares all member variables of this object and the argument for equality and returns true if all of them match
    public bool compareMemberVars(Food rhs)
    {
        if (this.currentSpread != rhs.currentSpread) return false;
        return true;
    }

    public void Cook()
    {
        cookAmount += Time.deltaTime;

        if (cookAmount >= timeToBurn)
        {
            condition = FoodCondition.Burnt;
        }
        else if (cookAmount >= timeToCook)
        {
            condition = FoodCondition.Cooked;
        }
    }
}
