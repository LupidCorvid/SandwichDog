using JetBrains.Annotations;
using UnityEngine;

public class Food : ObjClass
{
    public Spread currentSpread;
    public Food() : base(ObjType.PICKUP)
    {
        currentSpread = Spread.NOSPREAD;
    }
}
