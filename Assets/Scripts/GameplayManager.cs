using UnityEngine;


public class GameplayManager : MonoBehaviour
{
    public const int maxLevels = 1; //How many levels the game has
    public int currentLevel = 1; //The current level the player is playing
    public GameObject[] lvl1Reqs; //The objects required for scoring (prefabs dragged in via inspector)
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
    }
}
