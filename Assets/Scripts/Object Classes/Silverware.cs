using Unity.VisualScripting;
using UnityEngine;

public class Silverware : ObjClass
{
    [SerializeField] public bool isSlicer;

    public Silverware() : base(ObjType.GRABBABLE)
    {
        currentSpread = Spread.NOSPREAD;
    }

    private void OnCollisionEnter(Collision other)
    {
        ObjClass otherObject = other.gameObject.GetComponent<ObjClass>();

        if (!otherObject) return;

        Food foodObj = otherObject as Food;

        if (foodObj)
        {
            if (this.currentSpread != Spread.NOSPREAD && !foodObj.HasSpread && this.inHand)
            {
                otherObject.AddSpread(this.currentSpread);
            }
        }
    }
}
