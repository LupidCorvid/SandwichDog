using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialManager : Singleton<TutorialManager>
{
    //bool tutorialActive = false;
    int tutorialLevel = 0; //Different instructions for different levels

    int maxSize = 0;
    public int cursor = -1; //Where you currently are in the tutorial
    GameObject lastArrow = null;

    public TutorialAssignment [] levelTutorialObjects;
    //public GameObject[] lvl1Objects;  

    void Start()
    {
        //tutorialActive = false;
        //startTutorial(1);
    }

    float timer = 0f;
    
    void Update()
    {
        //Debug
        //timer += Time.deltaTime;
        //if (timer >= 3)
        //{
        //    timer = 0;
        //    advanceTutorial(1);
        //}
    }

    public void startTutorial(int level)
    {
        //tutorialActive = true;
        tutorialLevel = level;
        maxSize = levelTutorialObjects[tutorialLevel - 1].assignedTutorialObjs.Length; //7
        advanceTutorial(1);
    }

    //Once you interact with an object as instructed, it triggers the next tutorial event
    //It moves the cursor to match the current object touched, which accomodates for people who skip the tutorial
    public void askToAdvanceTutorial(int expectedCursor)
    {
        print("advtut " + expectedCursor);
        //The object must proc a tutorial that hasn't appeared yet
        if (expectedCursor > cursor)
        {
            int numSteps = Math.Abs(cursor - expectedCursor);
            advanceTutorial(numSteps);
        }
    }

    public void advanceTutorial(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            cursor += 1;
            if (cursor < maxSize)
            {
                //Spawn new stuff
                TutorialObj spawner = levelTutorialObjects[tutorialLevel - 1].assignedTutorialObjs[cursor];

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
                    print("arrow");
                    lastArrow = Instantiate(spawner.arrow, spawner.arrowPositionToSpawn, Quaternion.identity);
                    lastArrow.transform.eulerAngles = new Vector3(90, 90, 0);
                }
            }
            else break; //tutorialActive = false;
        }
    }
}
