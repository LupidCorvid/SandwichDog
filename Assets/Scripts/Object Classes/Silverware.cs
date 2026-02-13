using UnityEngine;

public class Silverware : ObjClass
{
    public Spread currentSpread;
    public Silverware() : base(ObjType.GRABBABLE)
    {
        currentSpread = Spread.NOSPREAD;
    }
}
