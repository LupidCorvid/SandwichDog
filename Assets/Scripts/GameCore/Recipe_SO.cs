using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_SO", menuName = "Scriptable Objects/Recipe_SO")]
public class Recipe_SO : ScriptableObject
{
    string recipeName;
    public Food[] requiredFood;
    //public China requiredPlate;
}
