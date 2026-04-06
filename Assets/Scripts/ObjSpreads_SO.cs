using Unity.VisualScripting;
using UnityEngine;

public struct SpreadInfo
{
    public Spread spread;
    public Material spreadMaterial;
}

[CreateAssetMenu(fileName = "ObjSpreads_SO", menuName = "Scriptable Objects/ObjSpreads_SO")]
public class ObjSpreads_SO : ScriptableObject
{
    public SpreadInfo[] spreads;

    public bool GetSpread(Spread spreadToFind, out Material matInfo)
    {
        foreach (SpreadInfo spread in spreads)
        {
            if (spread.spread == spreadToFind)
            {
                matInfo = spread.spreadMaterial;
                return true;
            }
        }
        matInfo = null;
        return false;
    }
}
