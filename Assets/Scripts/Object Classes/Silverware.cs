using Unity.VisualScripting;
using UnityEngine;

public class Silverware : ObjClass
{
    public Silverware() : base(ObjType.GRABBABLE)
    {
        currentSpread = Spread.NOSPREAD;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        if (col.tag == "Spread")
        {
            //If getting from a jar
            if (col.GetComponent<Jar>() != null && currentSpread == Spread.NOSPREAD)
            {
                AddSpread(col.GetComponent<Jar>().availSpread);
            }
        }
        else if (col.tag == "Sink")
        {
            RemoveSpreads();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.gameObject;
        
        if (col.tag == "Pickup")
        {
            //If Putting on food
            if (col.GetComponent<Food>() != null && currentSpread != Spread.NOSPREAD && inHand)
            {
                //Check if the food already has a spread before applying
                if (col.GetComponent<Food>().currentSpread == Spread.NOSPREAD)
                {
                    col.GetComponent<Food>().AddSpread(currentSpread);
                }
            }
        }
    }
}
