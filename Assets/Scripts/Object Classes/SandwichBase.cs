using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Rendering;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Rendering;

public class SandwichBase : MonoBehaviour
{
    [SerializeField] GameObject emptySandwichObject;
    [SerializeField] FoodStackCollider topStackCollider;
    [SerializeField] FoodStackCollider bottomStackCollider;
    private FoodStackCollider triggeredCollider;

    [SerializeField] Food baseFood;
    public Food BaseFood => baseFood;
    Food targetFood;
    Sandwich sandwich;

    // lerping info for prettier snapping over time
    [HideInInspector] public float timeToSnap;
    [HideInInspector] public float timeSpentSnapping;
    [HideInInspector] public float timeToSnapRemaining;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name + " is currently sitting in the trigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " definitely entered collider!");
        targetFood = other.GetComponentInChildren<Food>();

        if (!targetFood) return;
        if (!targetFood.isStackable) return;
        if (sandwich)
        {
            if (targetFood.transform.IsChildOf(sandwich.transform)) return;
        }

        if (!sandwich)
        {
            GameObject sandwichOwner = Instantiate(emptySandwichObject, this.transform);
            sandwichOwner.transform.SetParent(this.transform.parent, true);
            sandwich = sandwichOwner.GetComponent<Sandwich>();
            //topStackCollider.transform.SetParent(sandwich.transform);
            //sandwich.RigidBody.WakeUp();
            //Physics.SyncTransforms();
            //baseFood.DisableRigidBody();
            //targetFood.DisableRigidBody();
            baseFood.transform.SetParent(sandwich.transform);
            targetFood.transform.SetParent(sandwich.transform);
            Sandwich.SnapTo(baseFood, targetFood);

            // this is hacky but im not engineering this a diff way before URCAD
            //triggeredCollider = topStackCollider.stackCollider.bounds.Intersects(other.bounds) ? topStackCollider : bottomStackCollider;
            //// disable whichever collider didn't trigger the sandwich
            //if (triggeredCollider == topStackCollider)
            //{
            //    //bottomStackCollider.enabled = false;
            //}
            //else
            //{
            //    //topStackCollider.enabled = false;
            //}

            //triggeredCollider.transform.SetParent(sandwichOwner.transform, true);

            //triggeredCollider.enabled = false;
            //sandwich.InitializeSandwich(this, targetFood, triggeredCollider);
            //sandwich.EnableInteractability(); // starts as not interactable
        }
        else
        {
            Debug.Log("sandwich exists");
            targetFood.DisableRigidBody();
            targetFood.transform.SetParent(sandwich.transform);
            Sandwich.SnapTo(baseFood, targetFood);


            //triggeredCollider.enabled = false;
            //sandwich.PushNewFood(targetFood);
        }
        //triggeredCollider.transform.position += new Vector3(0.0f, (targetFood.topStackSnapPoint.transform.position.y - targetFood.transform.position.y), 0.0f);
        //triggeredCollider.transform.position += new Vector3(0.0f, (targetFood.objRenderer.bounds.extents.y), 0.0f);
        //triggeredCollider.enabled = true;
        // TODO nudge the remaining collider along with the snap
        // TODO setup so that position starts to lerp in update
    }

    public void HandleDestroySandwich()
    {

        Destroy(sandwich);
    }

    private void FixedUpdate()
    {
        // TODO, handle if target position is moving
        //if (timeToSnapRemaining > 0.0f)
        //{
        //    Vector3 newPosition = Vector3.Lerp(, , (timeSpentSnapping / timeToSnap));
        //    targetFood.transform.position = newPosition;

        //    targetFood.SnapTo(targetFood);

        //    timeSpentSnapping += Time.deltaTime;
        //    timeToSnapRemaining = (timeToSnap - timeSpentSnapping);
        //}
    }
}
