using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Rendering;

public class SandwichBase : MonoBehaviour
{
    [SerializeField] Food baseFood;
    public Food BaseFood => baseFood;

    [SerializeField] FoodStackCollider topStackCollider;
    [SerializeField] FoodStackCollider bottomStackCollider;
    public void DisableOtherTrigger(FoodStackCollider retainedCollider)
    {
        if (retainedCollider == topStackCollider)
        {
            bottomStackCollider.enabled = false;
        }
        else
        {
            topStackCollider.enabled = false;
        }
    }
}
