using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    bool tutorialActive = false;
    int tutorialLevel = 0; //Different instructions for different levels

    int maxSize = 0;
    int cursor = -1; //Where you currently are in the tutorial

    public TutorialAssignment levelTutorialObjects;

    void Start()
    {
        tutorialActive = false;
        maxSize = levelTutorialObjects.assignedTutorialObjs.Length;
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
