using UnityEngine;
using TMPro;
using System;

public class ClipboardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeRequirements;
    [SerializeField] private TMP_Text recipeDescription;

    private void Awake()
    {
        //LoadRecipe(GameplayManager.Instance.currentLevel.levelRecipe);
    }

    private void OnEnable()
    {
        //GameplayManager.OnRecipeProgressUpdated += UpdateRecipeText;
    }

    private void OnDisable()
    {
        //GameplayManager.OnRecipeProgressUpdated -= UpdateRecipeText;
    }

    private void LoadRecipe(Recipe_SO recipeToLoad)
    {
        //foreach (FoodRequirement itemRequirement in recipeToLoad.requiredFood)
        //{

        //}

        //String test = Spread.PEANUT_BUTTER.ToString();
        //test.Replace("_", " ");
        //"-covered" + 
    }

    public void UpdateRecipeText(ObjClass updatedObj)
    {

    }

    //scoreText.text = "Recipe Food:\n";
    //foreach (ItemRequirement itemRequirement in levelRecipe.requirements)
    //{
    //    scoreText.text += itemRequirement.item.m_name;

    //    Food food = itemRequirement.item as Food;

    //    if (food)
    //    {
    //        switch (food.currentSpread)
    //        {
    //            case ObjClass.Spread.NO_SPREAD:
    //                break;
    //            case ObjClass.Spread.PEANUT_BUTTER:
    //                scoreText.text += " with peanut butter";
    //                break;
    //            case ObjClass.Spread.JELLY:
    //                scoreText.text += " with jelly";
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    scoreText.text += ": " + itemRequirement.quantity + "\n";
    //}
}
