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

    [SerializeField] private SandwichBase sandwichBase;

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
        if (sandwichBase.enabled) sandwichBase.TryBuildSandwich(other.gameObject, this);
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
