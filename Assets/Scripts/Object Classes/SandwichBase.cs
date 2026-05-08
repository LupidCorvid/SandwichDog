using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Rendering;

public class SandwichBase : MonoBehaviour
{
    [SerializeField] Food baseFood;
    public Food BaseFood => baseFood;
    private bool isInitializing = false; // due to coroutine during initialization

    [SerializeField] GameObject emptySandwichObject;
    [SerializeField] public FoodStackCollider topStackCollider;
    [SerializeField] public FoodStackCollider bottomStackCollider;
    private Sandwich sandwich;
    public void TryBuildSandwich(GameObject incomingObject, FoodStackCollider triggeredCollider)
    {
        //Debug.Log(other.gameObject.name + " definitely entered collider!");
        SandwichBase otherSandwichBase;
        incomingObject.TryGetComponent<SandwichBase>(out otherSandwichBase);

        if (otherSandwichBase)
        {
            // prevent unintentional creation of sandwiches
            //if (!otherSandwichBase.BaseFood.inHand && !this.sandwichBase.BaseFood.inHand) return;

            // prevent forming purely bread sandwiches
            //if (otherSandwichBase.BaseFood.CurrentSpread == Spread.NO_SPREAD && this.sandwichBase.BaseFood.CurrentSpread == Spread.NO_SPREAD) return;

            // whichever sandwich base is moving more will relinquish ownership to the other
            if (this.BaseFood.RigidBody.linearVelocity.magnitude < otherSandwichBase.BaseFood.RigidBody.linearVelocity.magnitude)
            {
                FoodStackCollider closestOtherCollider = null;
                FoodStackCollider testCollider = null;
                if (triggeredCollider == topStackCollider)
                {
                    testCollider = topStackCollider;
                }
                else if (triggeredCollider ==  bottomStackCollider)
                {
                    testCollider = bottomStackCollider;
                }

                if (testCollider)
                {
                    if (Vector3.Distance(testCollider.transform.position, otherSandwichBase.topStackCollider.transform.position) <
                        Vector3.Distance(testCollider.transform.position, otherSandwichBase.bottomStackCollider.transform.position))
                    {
                        closestOtherCollider = otherSandwichBase.topStackCollider;
                    }
                    else
                    {
                        closestOtherCollider = otherSandwichBase.bottomStackCollider;
                    }

                    otherSandwichBase.BuildSandwich(otherSandwichBase, this.BaseFood, closestOtherCollider);
                    this.DisableBothTriggers();
                }
            }
        }

        Food targetFood = incomingObject.GetComponentInChildren<Food>();

        if (!targetFood) return;
        if (!targetFood.isStackable) return;
        if (!targetFood.inHand && !this.BaseFood.inHand) return;
        if (sandwich)
        {
            if (targetFood.transform.IsChildOf(sandwich.transform)) return;
        }

        if (!sandwich && !isInitializing)
        {
            isInitializing = true;
            BuildSandwich(this, targetFood, triggeredCollider);
        }
        else if (sandwich)
        {
            Debug.Log("sandwich exists");
            sandwich.PushNewFood(targetFood);

        }
        // TODO setup so that position starts to lerp in update
    }

    public void BuildSandwich(SandwichBase sandwichBase, Food targetFood, FoodStackCollider triggeredCollider)
    {
        this.DisableOtherTrigger(triggeredCollider);

        GameObject sandwichOwner = Instantiate(emptySandwichObject);
        sandwichOwner.transform.position = this.transform.position;
        sandwich = sandwichOwner.GetComponent<Sandwich>();

        sandwich.InitializeSandwich(sandwichBase, targetFood, triggeredCollider);
        sandwich.EnableInteractability(); // starts as not interactable
    }

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

    // called when another SandwichBase has taken ownership of this
    public void DisableBothTriggers()
    {
        bottomStackCollider.enabled = false;
        topStackCollider.enabled = false;
    }
}
