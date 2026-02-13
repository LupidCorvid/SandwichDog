using JetBrains.Annotations;
using UnityEngine;

public class Food : ObjClass
{
    public Spread currentSpread;
    public GameObject mesh;
    public Material[] matSpreads;

    public Food() : base(ObjType.PICKUP)
    {
        currentSpread = Spread.NOSPREAD;
    }

    public void addSpread(Spread s)
    {
        currentSpread = s;
        if (mesh != null)
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
}
