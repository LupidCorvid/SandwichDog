using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class tempPageTurner : MonoBehaviour
{
    private XRGrabInteractable xrgi;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xrgi = gameObject.GetComponent<XRGrabInteractable>();
        xrgi.selectEntered.AddListener(PickedUp);
        xrgi.selectExited.AddListener(PutDown);
    }

    public void PickedUp(SelectEnterEventArgs args)
    {
        player.GetComponent<Player>().heldObject = gameObject;
    }
    public void PutDown(SelectExitEventArgs args)
    {
        player.GetComponent<Player>().heldObject = null;
    }
}
