using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_SO", menuName = "Scriptable Objects/Recipe_SO")]
public class Recipe_SO : ScriptableObject
{
    public ItemRequirement[] requirements;
}
