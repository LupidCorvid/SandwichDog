using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TestCollider : MonoBehaviour
{
    private List<Food> seenFood = new List<Food>();
    public Collider collider;
    Food previousFood;
    Food targetFood;

    private void Awake()
    {
        previousFood = this.transform.parent.GetComponent<Food>();
        seenFood.Add(previousFood);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " definitely entered collider!");
        targetFood = other.GetComponentInChildren<Food>();

        if (!targetFood) return;
        if (!targetFood.isStackable) return;

        if (seenFood.Exists(food => ReferenceEquals(food, targetFood))) return;

        if (gameObject.transform.parent)
        {
            this.gameObject.transform.SetParent(this.gameObject.transform.parent.parent);
        }

        targetFood.DisableRigidBody();
        Sandwich.SnapTo(previousFood, targetFood);
        seenFood.Add(targetFood);

        previousFood = targetFood;
        targetFood = null;
        //Destroy(targetFood.gameObject);
    }
}