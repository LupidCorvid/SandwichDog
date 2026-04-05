using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    bool tutorialActive = false;
    int tutorialLevel = 0; //Different instructions for different levels

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject instructionUI;

    [SerializeField] Vector3[] arrowLocations; //List of where to spawn the arrows
    [SerializeField] Vector3[] UILocations;     //List of where to spawn the instructionUI
    int locationListSize = 0;
    int cursor = -1; //Where you currently are in the tutorial

    void Start()
    {
        tutorialActive = false;
        locationListSize = arrowLocations.Length;
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

    //It's just a switch statement </3
    /*  have a place to spawn the arrow
        have a place to spawn the UI
        just step through the parallel array */
    public void advanceTutorial()
    {
        cursor += 1;
        switch (cursor)
        {
            /* On level start, place the dog right in front of the table. 
             * The table has the book on it. There's an arrow and billboarded 
             * UI that says “hold __ and ___ at the same time to pick up objects”.
             */
            case 0:
                break;

            /* When the player picks it up, the UI makes a sound and changes text 
             * to say “press __ to turn the page forward, and __ to turn the page backward”*/
            case 1:
                break;

            /* At the entrance to the kitchen, there’s a UI that says “press __ to swap walking style” */
            case 2:
                break;

            /* After the player makes it to the last page OR walks into the kitchen 
             * (whichever comes first), the same arrow and UI combination appears 
             * above the clipboard, which is on the island */
            case 3:
                break;

            /* After the player interacts with the knife, the arrow moves to a cabinet, 
             * and says “cabinets can be opened with one paw with __ or __”. */
            case 4:
                break;

            /* After the player interacts with the cabinet, the arrow moves to the table 
             * and there’s UI that says “when your sandwich is done, put it on a plate and place it on the table */
            case 5:
                break;

            default:
                break;
        }
    }
}
