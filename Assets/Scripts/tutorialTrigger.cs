using UnityEngine;

public class tutorialTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] int advanceCount; //How many steps to advance the tutorial to make it to the Ui that shows at this trigger specifically
                                       //This is just in case the player doesn't interact with the book and walks into the kitchen immediately

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (tutorialManager.cursor < advanceCount)
            {
                int count = advanceCount - tutorialManager.cursor; //lvl = 2
                tutorialManager.advanceTutorial(count);
            }
            Destroy(gameObject);
        }
    }
}
