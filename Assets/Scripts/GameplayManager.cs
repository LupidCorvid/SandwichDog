using UnityEngine;
using System.Collections.Generic;


public class GameplayManager : MonoBehaviour
{
    public const int maxLevels = 1; //How many levels the game has
    public int currentLevel = 1; //The current level the player is playing

    private List<ObjClass> lvlReqs; //The objects required for scoring 
    private List<GameObject> objectsToScore = new List<GameObject>(); //The objects the player brought to the end game area (should be added via this script)

    private float timer = 5; //Time to wait for the player to stand in the box before scoring objects

    void Start()
    {
        LoadReqs(currentLevel);
    }

    void Update()
    {
        if (timer <= 0) CalculateScore();
    }

    public void CalculateScore()
    {
        //First check which level the game is on
        //If level 1...
        //Check that the object is named "Bread"
        //Check that one object has peanutbutter and one has jelly from Food (I.e. Food.Spread.JELLY)
        if (currentLevel == 1)
        {
            if (objectsToScore[objectsToScore.Length].name == "Bread")
            {
                if (objectsToScore[0].GetComponent<Food>().currentSpread == Food.Spread.PEANUTBUTTER && objectsToScore[0].GetComponent<Food>().currentSpread == Food.Spread.JELLY)
                {
                    Debug.Log("You win!");
                }
                else
                {
                    Debug.Log("You lose! You need peanut butter and jelly on your bread.");
                }
            }
            else
            {
                Debug.Log("You lose! You need to bring bread to the end game area.");
            }
        }
        //If level 2...
    }

    //When the player enters, wait for 5 seconds and then score
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer -= Time.deltaTime;
        }
    }

    //If the player leaves too early, reset the timer
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && timer > 0)
        {
            timer = 5;
        }
    }

    //Add objects to the objectsToScore array
    //TODO: Curretly doesn't remove objects that leave onTriggerExit
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            CalculateScore();
            Debug.Log("Object added to score: " + other.gameObject.name);

            objectsToScore.Add(other.gameObject);
        }
    }

    //Loads the required objects for the level
    void LoadReqs(int lvl)
    {
        switch (lvl)
        {
            case 1:
            {
                    //PB bread
                    Food bread1 = new Food();
                    bread1.currentSpread = ObjClass.Spread.PEANUTBUTTER;
                    lvlReqs.Add(bread1);

                    //Jelly Bread
                    Food bread2 = new Food();
                    bread2.currentSpread = ObjClass.Spread.JELLY;
                    lvlReqs.Add(bread2);

                    break;
            }  
        }
    }
}
