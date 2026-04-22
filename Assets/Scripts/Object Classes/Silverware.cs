using Unity.VisualScripting;
using UnityEngine;

public class Silverware : ObjClass
{
    [SerializeField] public bool isSlicer;

    public Silverware() : base(ObjType.GRABBABLE)
    {
        //currentSpread.spread = Spread.NO_SPREAD;
    }

    private void OnCollisionEnter(Collision other)
    {
        ObjClass otherObject = other.gameObject.GetComponent<ObjClass>();

        if (!otherObject) return;

        Food foodObj = otherObject as Food;

        if (foodObj)
        {
            //if (this.inHand && this.HasSpread && !foodObj.HasSpread)
            if (this.HasSpread && !foodObj.HasSpread)
            {
                otherObject.AddSpread(this.currentSpread, this.transform);
            }
            if (foodObj.IsSliceable && this.isSlicer)
            {
                foodObj.Slice();
            }
        }
    }
}
