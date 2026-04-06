using UnityEngine;

[CreateAssetMenu(fileName = "GlobalObjSettings_SO", menuName = "Scriptable Objects/GlobalObjSettings_SO")]
public class GlobalObjSettings_SO : ScriptableObject
{
    public Material dirtMaterial;
    public float amountToDirtyPerSecond;
    public float amountToCleanPerSecond;

    public static string GetPath()
    {
        return "Global/GlobalObjSettings_SO";
    }
}
