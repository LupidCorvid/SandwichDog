using System;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Instruction
{
    enum InstructionType
    {
        SPREAD,
        COOK_PAN,

        CONSTRUCT,
        DELIVER
    }

    public void SetInstructionText(string msg)
    {
        instructionText.text = msg;
    }

    // dynamic checkbox scrpaped for URCAD
    //[SerializeField] Image checkboxImage;
    [SerializeField] TMP_Text instructionText;
    //Food foodState;

    public TMP_Text Text => instructionText;

    //    public void MarkComplete()
    //    {
    //        checkboxImage.enabled = true;
    //    }
    //    public void MarkIncomplete()
    //    {
    //        checkboxImage.enabled = true;
    //    }
    //}
}

public class ClipboardUI : MonoBehaviour
{
    private GameObject instructionTemplateObj;
    private Instruction instructionTemplate;

    [SerializeField] private Canvas clipboardCanvas;
    [SerializeField] private TMP_Text instructionsHeader;
    //private List<Instruction> instructions;

    [SerializeField] private float spaceBetweenLines;
    private int maxNumLinesPerPage;
    private int maxNumWordsPerLine;


    private void Awake()
    {
        instructionTemplate = instructionTemplateObj.GetComponent<Instruction>();
        // canvas height / (instruction height+space between instructs) = num instructions per page
        maxNumLinesPerPage = (int)(LayoutUtility.GetPreferredHeight((RectTransform)clipboardCanvas.transform) / instructionTemplate.Text.preferredHeight);

        LoadRecipe(GameplayManager.Instance.gameLevel.levelRecipe);
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
        int instructionNum = 1;
        string instructionMsg = "";

        instructionsHeader.text = recipeToLoad.name;

        foreach (Food recipeFood in recipeToLoad.requiredFood)
        {
            instructionMsg = instructionNum.ToString() + ". ";

            if (recipeFood.IsCookable)
            {
                instructionMsg += "Cook the " + recipeFood.name;
            }

            if (recipeFood.HasSpread)
            {
                string spreadName = recipeFood.currentSpread.ToString().Replace("_", " ");
                instructionMsg += "Spread " + spreadName + " on the " + recipeFood.name;
            }
            if (recipeFood.SliceSource)
            {
                instructionMsg += "Chop the " + recipeFood.SliceSource.name;
            }

            PushNewInstruction(instructionMsg);
            instructionNum++;
        }
        instructionMsg = instructionNum.ToString() + ". ";
        instructionMsg += "Deliver the food to your owner!";
    }

    private void PushNewInstruction(string instruction)
    {
        GameObject instructionObj = Instantiate(instructionTemplateObj);
        instructionObj.transform.SetParent(this.transform);
    }
}
