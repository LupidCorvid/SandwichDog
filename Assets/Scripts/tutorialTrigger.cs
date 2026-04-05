using UnityEngine;

public class tutorialTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tutorialManager.advanceTutorial();
            Destroy(gameObject);
        }
    }
}
