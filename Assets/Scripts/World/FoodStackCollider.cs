using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class FoodStackCollider : MonoBehaviour
{
    public Collider stackCollider;

    private void OnDisable()
    {
        stackCollider.enabled = false;
    }

    private void OnEnable()
    {
        stackCollider.enabled = true;
    }
}
