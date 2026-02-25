using UnityEngine;


public class GameplayManager : MonoBehaviour
{
    public const int maxLevels = 2; //How many levels the game has
    public int currentLevel = 1; //The current level the player is playing
    public GameObject[] lvl1Reqs; //The objects required for scoring (prefabs dragged in via inspector)
    public GameObject[] lvl2Reqs; //The objects required for scoring (prefabs dragged in via inspector)
    public GameObject[] objectsToScore; //The objects the player brought to the end game area (should be added via this script)



    void Start()
    {
    }

    void Update()
    {
        
    }

    public void CalculateScore()
    {
        //First check which level the game is on
        //If level 1...
        //Check that the object is named "Bread"
        //Check that one object has peanutbutter and one has jelly from Food (I.e. Food.Spread.JELLY)
        if (currentLevel == 1)
        {
            if (objectsToScore[objectsToScore.Length - 1].name == "Bread")
            {/*
                if (objectsToScore[0].GetComponent<Food>().spread == Food.Spread.PEANUTBUTTER && objectsToScore[1].GetComponent<Food>().spread == Food.Spread.JELLY)
                {
                    Debug.Log("You win!");
                }
                else
                {
                    Debug.Log("You lose! You need peanut butter and jelly on your bread.");
                }*/
            }
            else
            {
                Debug.Log("You lose! You need to bring bread to the end game area.");
            }
        }
        //If level 2...
    }

    void OnTriggerEnter(Collider other)
    {
        //When the player enters the end game area, add the object they are holding to the objectsToScore array
        if (other.gameObject.CompareTag("Pickup"))
        {
            Debug.Log("Object added to score: " + other.gameObject.name);
            objectsToScore[objectsToScore.Length - 1] = other.gameObject;
        }
    }
}
