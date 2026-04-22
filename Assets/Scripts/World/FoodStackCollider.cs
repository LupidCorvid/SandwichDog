using UnityEngine;
using System.Collections;

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

    [SerializeField] GameObject emptySandwichObject;

    public SandwichBase sandwichBase;
    Sandwich sandwich;
    private bool isInitializing = false; // due to coroutine during initialization

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
        Food targetFood = other.GetComponentInChildren<Food>();

        if (!targetFood) return;
        if (!targetFood.isStackable) return;
        if (sandwich)
        {
            if (targetFood.transform.IsChildOf(sandwich.transform)) return;
        }

        if (!sandwich && !isInitializing)
        {
            isInitializing = true;
            sandwichBase.DisableOtherTrigger(this);
            
            GameObject sandwichOwner = Instantiate(emptySandwichObject);
            sandwichOwner.transform.position = this.transform.position;
            sandwich = sandwichOwner.GetComponent<Sandwich>();

            sandwich.InitializeSandwich(this.sandwichBase, targetFood, this);
            sandwich.EnableInteractability(); // starts as not interactable
        }
        else
        {
            Debug.Log("sandwich exists");
            sandwich.PushNewFood(targetFood);

        }
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
