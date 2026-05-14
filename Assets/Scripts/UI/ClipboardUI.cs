using System;
using System.Collections.Generic;
using System.Linq;
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
    private const int MIN_INSTRUCT_LEN = 3;

    //private GameObject instructionTemplateObj;
    //private Instruction instructionTemplate;

    [SerializeField] private Canvas clipboardCanvas;
    [SerializeField] private TMP_Text instructionsHeader;

    //private List<Instruction> instructions;

    //[SerializeField] private float spaceBetweenLines;
    //private int maxNumLinesPerPage;
    //private int maxNumWordsPerLine;


    private void Awake()
    {
        //instructionTemplate = instructionTemplateObj.GetComponent<Instruction>();
        // canvas height / (instruction height+space between instructs) = num instructions per page
        //maxNumLinesPerPage = (int)(LayoutUtility.GetPreferredHeight((RectTransform)clipboardCanvas.transform) / instructionTemplate.Text.preferredHeight);

    }

    private void Start()
    {
        LoadRecipeToClipboard(GameplayManager.Instance.gameLevel.levelRecipe);
    }

    private void OnEnable()
    {
        //GameplayManager.OnRecipeProgressUpdated += UpdateRecipeText;
    }

    private void OnDisable()
    {
        //GameplayManager.OnRecipeProgressUpdated -= UpdateRecipeText;
    }

    private void LoadRecipeToClipboard(Recipe_SO recipeToLoad)
    {
        List<string> instructions = new List<string>();
        int instructionNum = 1;
        string clipboardText = "";

        //instructionsHeader.text = recipeToLoad.name;

        foreach (FoodRequirement recipeReq in recipeToLoad.requiredFood)
        {
            string fullInstruction = "";
            string instructionInfo = "";

            Food recipeFood = recipeReq.food;

            fullInstruction += instructionNum.ToString() + ". ";

            if (recipeReq.isCooked)
            {
                if (recipeFood.ObjName == "Bread")
                {
                    instructionInfo += "Toast the " + recipeFood.name;
                }
                else
                {
                    instructionInfo += "Cook the " + recipeFood.name;
                }
            }

            if (recipeReq.spread != Spread.NO_SPREAD)
            {
                string spreadName = recipeReq.spread.ToString().Replace("_", " ").ToLower();
                instructionInfo += "Spread the " + spreadName + " on the " + recipeFood.name;
            }
            if (recipeFood.SliceSource)
            {
                instructionInfo  += "Chop the " + recipeFood.SliceSource.name;
            }

            //PushNewInstruction(instructionMsg);
            if (instructionInfo.Length > 0)
            {
                // only add novel instructions
                if (!instructions.Any(instruction => instruction == instructionInfo))
                {
                    instructions.Add(instructionInfo);
                    fullInstruction += instructionInfo;
                    clipboardText += fullInstruction + "\n";
                    instructionNum++;
                }
            }
        }

        clipboardText += "Construct the sandwich!";
        clipboardText += "\n";
        instructionNum++;


        clipboardText += instructionNum.ToString() + ". ";
        clipboardText += "Deliver the food to your owner!";

        instructionsHeader.text = clipboardText;
    }

    //private void PushNewInstruction(string instruction)
    //{
    //    GameObject instructionObj = Instantiate(instructionTemplateObj);
    //    instructionObj.transform.SetParent(this.transform);
    //}
}

