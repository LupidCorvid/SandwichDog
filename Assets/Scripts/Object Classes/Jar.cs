using UnityEngine;

public class Jar : ObjClass
{
    public Spread availSpread;
    public Jar() : base(ObjType.PICKUP)
    {
        availSpread = Spread.PEANUTBUTTER;
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjClass otherObject = other.gameObject.GetComponent<ObjClass>();
        if (!otherObject) return;

        Silverware silverwareObj = otherObject as Silverware;

        // apply jar spread to silverware
        if (silverwareObj)
        {
            if (!silverwareObj.HasSpread)
            {
                silverwareObj.AddSpread(this.availSpread);
            }
        }
    }
}
