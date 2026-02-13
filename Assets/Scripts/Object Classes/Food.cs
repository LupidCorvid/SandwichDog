using JetBrains.Annotations;
using UnityEngine;

public class Food : ObjClass
{
    public Spread currentSpread;
    public Food() : base(ObjType.PICKUP)
    {
        currentSpread = Spread.NOSPREAD;
    }

    public void addSpread(Spread s)
    {
        currentSpread = s;
        //TODO: change material
    }
    public void removeSpread()
    {
        currentSpread = Spread.NOSPREAD;
        //TODO: remove material
    }
}
