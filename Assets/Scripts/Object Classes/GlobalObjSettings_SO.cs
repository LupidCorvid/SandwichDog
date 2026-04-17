using UnityEngine;

[CreateAssetMenu(fileName = "GlobalObjSettings_SO", menuName = "Scriptable Objects/GlobalObjSettings_SO")]
public class GlobalObjSettings_SO : ScriptableObject
{
    public bool canGetDirty;
    public Material dirtMaterial;
    public float amountToDirtyPerSecond;

    public bool canGetClean;
    public float amountToCleanPerSecond;

    public static string DefaultObjPath = "Global/GlobalObjSettings_SO";
    public static string FoodObjPath = "Global/GlobalFoodSettings_SO";
    public static string SilverwareObjPath = "Global/GlobalSilverwareSettings_SO";
}
