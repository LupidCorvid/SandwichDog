using UnityEngine;

public class Jar : ObjClass
{
    public Spread availSpread;
    public Jar() : base(ObjType.PICKUP)
    {
        availSpread = Spread.PEANUTBUTTER;
    }
}
