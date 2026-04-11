using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    bool tutorialActive = false;
    int tutorialLevel = 0; //Different instructions for different levels

    int maxSize = 0;
    public int cursor = -1; //Where you currently are in the tutorial
    GameObject lastArrow = null;

    List<string> itemsThatTriggeredTutorial = new List<string>();

    public TutorialAssignment levelTutorialObjects;

    public string[] lvl1Text = {
        "Press and Hold both Side Triggers at the same time to pick up objects.",
        "Press B to turn the page forward, and Y to turn the page backward.",
        "Press X to swap walking style.",
        "Press and Hold both Side Triggers at the same time to pick up objects.",
        "Silverware can be held in one paw with the left or right Side Trigger.",
        "Cabinets can be opened with one paw with the left or right Side Trigger.",
        "When your sandwich is done, put it on a plate and place it on the table in the living room."};

    void Start()
    {
        tutorialActive = false;
        maxSize = levelTutorialObjects.assignedTutorialObjs.Length; //7
        startTutorial(1);
    }

    float timer = 0f;
    
    void Update()
    {

        //Debug
        //timer += Time.deltaTime;
        //if (timer >= 3)
        //{
        //    timer = 0;
        //    advanceTutorial();
        //}
    }

    public void startTutorial(int level)
    {
        tutorialActive = true;
        tutorialLevel = level;
        advanceTutorial();
    }

    //TODO test on book
    public void askToAdvanceTutorial(string ID)
    {
        if (!itemsThatTriggeredTutorial.Contains(ID))
        {
            advanceTutorial();
            itemsThatTriggeredTutorial.Add(ID);
            //if (ID == "book") advanceTutorial(); //debug
            if (ID == "butterknife") advanceTutorial(); //debug
        }
    }

    public void advanceTutorial()
    {
        cursor += 1;
        if (cursor < maxSize)
        {
            //Spawn new stuff
            TutorialObj spawner = levelTutorialObjects.assignedTutorialObjs[cursor];
            
            //Delete the last arrow (keep UI text on screen for players to reference again)
            if (lastArrow != null) Destroy(lastArrow);

            //Some tutorial prompts dont require UI text. Position (0, 0, 0) signifies this.
            if (spawner.UIPositionToSpawn != Vector3.zero)
            {
                GameObject instructionUI = Instantiate(spawner.instructionUI, spawner.UIPositionToSpawn, Quaternion.identity);
                instructionUI.GetComponentInChildren<Text>().text = spawner.UIText;
                instructionUI.transform.Find("ControllerImage1").GetComponent<Image>().sprite = spawner.controllerImg1;
                instructionUI.transform.Find("ControllerImage2").GetComponent<Image>().sprite = spawner.controllerImg2;
            }

            //Some tutorial prompts dont require an arrow. Position (0, 0, 0) signifies this.
            if (spawner.arrowPositionToSpawn != Vector3.zero)
            {
                lastArrow = Instantiate(spawner.arrow, spawner.arrowPositionToSpawn, Quaternion.identity);
                lastArrow.transform.eulerAngles = new Vector3(90, 90, 0);
            }

            
        }
        else tutorialActive = false;
    }
}
