using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CookType
{
    Pan,
    Bake
}

public class CookCollider : MonoBehaviour
{
    [SerializeField] private CookType cookType;

    private List<Food> cookingFood = new List<Food>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("new item in cooker: " + other.gameObject.name);

        Food newFood = other.gameObject.GetComponent<ObjClass>() as Food;
        if (!newFood) return;

        if (newFood.isCookable)
        {
            cookingFood.Add(newFood);
            newFood.cookTimeStart = Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("removing item in cooker: " + other.gameObject.name);

        Food newFood = other.gameObject.GetComponent<ObjClass>() as Food;
        if (!newFood) return;

        if (newFood.isCookable)
        {
            cookingFood.Remove(newFood);
        }
    }

    private void FixedUpdate()
    {
        foreach (Food food in cookingFood)
        {
            food.Cook();
        }
    }
}
