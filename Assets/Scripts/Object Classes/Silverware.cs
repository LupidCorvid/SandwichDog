using UnityEngine;

public class Silverware : ObjClass
{
    public Spread currentSpread;
    public Silverware() : base(ObjType.GRABBABLE)
    {
        currentSpread = Spread.NOSPREAD;
    }
    public void addSpread(Spread s)
    {
        currentSpread = s;
    }
    public void removeSpread()
    {
        currentSpread = Spread.NOSPREAD;
    }
}
