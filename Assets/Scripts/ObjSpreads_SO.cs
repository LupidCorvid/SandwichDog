using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SpreadInfo
{
    public Spread spread;
    public GameObject spreadObject;
    public Material spreadMaterial;
}

[CreateAssetMenu(fileName = "ObjSpreads_SO", menuName = "Scriptable Objects/ObjSpreads_SO")]
public class ObjSpreads_SO : ScriptableObject
{
    public SpreadInfo[] spreads;

    public SpreadInfo GetSpreadInfo(Spread spreadToFind)
    {
        foreach (SpreadInfo spreadInfo in spreads)
        {
            if (spreadInfo.spread == spreadToFind)
            {
                return spreadInfo;
            }
        }
        return null;
    }
}
