using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialInvoker : MonoBehaviour
{
    public int expectedCursor; //What stage of the tutorial the object expects to be triggered at
    public int expectedLevel; //What level this tutorial object should proc at
    public enum TriggerType { PICKUP, INTERACT, PASS_THROUGH};
    public TriggerType triggerType;
    private bool levelMatches;
    public bool destroyIfLevelDoesntMatch;
    private XRGrabInteractable xrgi;


    void Start()
    {
        levelMatches = GameplayManager.Instance.currentLevel == expectedLevel;
        //This object should not be seen if the level doesn't match
        if (!levelMatches && destroyIfLevelDoesntMatch)
        {
            Destroy(gameObject);
        }
        //This object should be seen but shouldn't proc the tutorial
        else if (!levelMatches)
        {
            gameObject.GetComponent<TutorialInvoker>().enabled = false;
        }
        //This object should proc the tutorial when picked up
        else
        {
            if (triggerType == TriggerType.PICKUP) {
                xrgi = gameObject.GetComponent<XRGrabInteractable>();
                xrgi.selectEntered.AddListener(TriggerTutorialSelect);
            }
            else if (triggerType == TriggerType.INTERACT) //GO must have a box collider
            {
                xrgi.activated.AddListener(TriggerTutorialActivate);
            }
        }
    }

    private void TriggerTutorialSelect(SelectEnterEventArgs args)
    {
        TutorialManager.Instance.askToAdvanceTutorial(expectedCursor);
        gameObject.GetComponent<TutorialInvoker>().enabled = false;     //Disable to make sure it doesn't get proc'd again
    }
    private void TriggerTutorialActivate(ActivateEventArgs args)
    {
        TutorialManager.Instance.askToAdvanceTutorial(expectedCursor);
        gameObject.GetComponent<TutorialInvoker>().enabled = false;     //Disable to make sure it doesn't get proc'd again
    }

    private void OnTriggerEnter(Collider other)
    {
        if(triggerType == TriggerType.PASS_THROUGH)
        {
            if (other.tag == "Player")
            {
                TutorialManager.Instance.askToAdvanceTutorial(expectedCursor);
                gameObject.GetComponent<TutorialInvoker>().enabled = false;     //Disable to make sure it doesn't get proc'd again
            }
        }
    }
}
