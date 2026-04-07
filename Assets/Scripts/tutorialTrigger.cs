using UnityEngine;

public class tutorialTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //This is just in case the player doesn't interact with the book and walks into the kitchen immediately
            //int advanceCount = 2 - tutorialManager.cursor;
            //for (int i = 0; i < advanceCount; i++)
            //{
                tutorialManager.advanceTutorial();
            //}
            Destroy(gameObject);
        }
    }
}
