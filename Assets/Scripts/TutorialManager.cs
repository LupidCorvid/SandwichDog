using UnityEngine;
using System;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    bool tutorialActive = false;
    int tutorialLevel = 0; //Different instructions for different levels

    int maxSize = 0;
    public int cursor = -1; //Where you currently are in the tutorial

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
    }

    
    void Update()
    {
        
    }

    public void startTutorial(int level)
    {
        tutorialActive = true;
        tutorialLevel = level;
        advanceTutorial();
    }

    public void advanceTutorial()
    {
        cursor += 1;
        if (cursor < maxSize)
        {
            //Spawn new stuff
            TutorialObj spawner = levelTutorialObjects.assignedTutorialObjs[cursor];
            Instantiate(spawner.arrow, spawner.arrowPositionToSpawn, Quaternion.identity);
            Instantiate(spawner.instructionUI, spawner.UIPositionToSpawn, Quaternion.identity);
        }
        else tutorialActive = false;
    }
}
