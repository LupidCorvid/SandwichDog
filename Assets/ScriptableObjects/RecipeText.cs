using UnityEngine;

[System.Serializable]
public struct RecipeString
{
    public string[] ingredients;
    public string[] steps;
    public string bulletPoint;
    public string combinedText;

    public RecipeString(string[] in_ingredients, string[] in_steps)
    {
        ingredients = in_ingredients;
        steps = in_steps;
        bulletPoint = "-";
        combinedText = "Ingredients:";

        foreach (string item in ingredients)
        {
            combinedText += "\n" + bulletPoint + item;
        }
        combinedText += "Steps:";
        foreach (string item in steps)
        {
            combinedText += "\n" + bulletPoint + item;
        }
    }
}

[CreateAssetMenu(fileName = "RecipeText", menuName = "Scriptable Objects/RecipeText")]
public class RecipeText : ScriptableObject
{
    public RecipeString[] assignedRecipes;
}
