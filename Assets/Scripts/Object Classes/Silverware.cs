using UnityEngine;

public class Silverware : ObjClass
{
    public Spread currentSpread;
    public GameObject mesh;
    public Material[] matSpreads;

    public Silverware() : base(ObjType.GRABBABLE)
    {
        currentSpread = Spread.NOSPREAD;
    }
    public void addSpread(Spread s)
    {
        //All spreads are shared as an array in the editor. The spread you want to add should correspond with the enum order.
        currentSpread = s;
        if(mesh != null)
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.gameObject;

        if (col.tag == "Grabbable")
        {
            //If getting from a jar
            if (col.GetComponent<Jar>() != null && currentSpread == Spread.NOSPREAD)
            {
                addSpread(col.GetComponent<Jar>().availSpread);
                return;
            }
        }
        else if (col.tag == "Pickup")
        {
            //If Putting on food
            if (col.GetComponent<Food>() != null && currentSpread != Spread.NOSPREAD && inHand)
            {
                col.GetComponent<Food>().addSpread(currentSpread);
                removeSpread();
                return;
            }
        }
    }
}
