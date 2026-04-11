using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public const int maxLevels = 1; //How many levels the game has
    public int currentLevel = 1; //The current level the player is playing

    public Recipe_SO levelRecipe;

    //The objects the player brought to the end game area, added via end area OnTriggerEnter
    public List<ItemRequirement> objectsToScore = new List<ItemRequirement>();

    public Text scoreText;
    public Text displayText;

    public float timer; //Time to wait in seconds for the player to stand in the box before scoring objects
    public float scoreMax = 0.0f;
    public float score = 0;
    public bool gameOver;

    public static event Action onScoreCalculate;
    [SerializeField] TutorialManager tutorialManager;

    private void Awake()
    {
        gameOver = false;
        displayText.text = "0%";
        scoreMax = levelRecipe.requirements.Length;

        


        //scoreText.text = "Recipe Food:\n";
        //foreach (ItemRequirement itemRequirement in levelRecipe.requirements)
        //{
        //    scoreText.text += itemRequirement.item.m_name;

        //    Food food = itemRequirement.item as Food;

        //    if (food)
        //    {
        //        switch (food.currentSpread)
        //        {
        //            case ObjClass.Spread.NOSPREAD:
        //                break;
        //            case ObjClass.Spread.PEANUTBUTTER:
        //                scoreText.text += " with peanut butter";
        //                break;
        //            case ObjClass.Spread.JELLY:
        //                scoreText.text += " with jelly";
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    scoreText.text += ": " + itemRequirement.quantity + "\n";
        //}
    }

    void Start()
    {
        /*if (currentLevel == 1) */tutorialManager.startTutorial(currentLevel);
    }

    private void OnEnable()
    {
        onScoreCalculate += CalculateScore;
    }

    void Update()
    {

    }

    public void CalculateScore()
    {
        List<ItemRequirement> itemRequirements = levelRecipe.requirements.ToList();

        float scoreCount = 0;
        foreach (ItemRequirement objectToScore in objectsToScore)
        {
            //Debug.Log(objectToScore.item.gameObject.GetComponent<Rigidbody>() == null);

            Debug.Log(objectToScore.item.objCleanliness);
            scoreCount += objectToScore.item.objCleanliness * objectToScore.quantity;
        }
        score = (scoreCount / scoreMax) * 100.0f;

        //Update score text
        Debug.Log("Score:" + score);
        displayText.text = score.ToString("F2").Truncate(5) + "%";

        //First check which level the game is on
        //If level 1...
        //Check that the object is named "bread"
        //Check that one object has peanutbutter and one has jelly from Food (I.e. Food.Spread.JELLY)

        //if (currentLevel == 1)
        //{
        //    if (objectsToScore[objectsToScore.Length].name == "Bread")
        //    {
        //        if (objectsToScore[0].GetComponent<Food>().currentSpread == Food.Spread.PEANUTBUTTER && objectsToScore[0].GetComponent<Food>().currentSpread == Food.Spread.JELLY)
        //        {
        //            Debug.Log("You win!");
        //        }
        //        else
        //        {
        //            Debug.Log("You lose! You need peanut butter and jelly on your bread.");
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("You lose! You need to bring bread to the end game area.");
        //    }
        //}
    }

    //When the player enters, wait for 5 seconds and then score
    private void OnTriggerStay(Collider other)
    {
        //DEBUG checks if the plate entered the scoring layer
        //Change it to let the player click a UI element to start the scoring
        if (other.gameObject.layer == 10) //if (other.gameObject.CompareTag("Player")) //if (other.gameObject.layer == 10)
        {
            timer -= Time.deltaTime;
            //if(timer > 4) displayText.text = "Hold still";
            //else if(timer > 3) displayText.text = "Hold still.";
            //else if (timer > 2) displayText.text = "Hold still..";
            //else if (timer > 1) displayText.text = "Hold still...";

            if (timer <= 0)
            {
                onScoreCalculate?.Invoke();
            }
        }
    }

    //If the player leaves too early, reset the timer
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("removing item in scoring area: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player") && timer > 0)
        {
            timer = 5;
            displayText.text = "Stand here";
        }
    }

    //Add objects to the objectsToScore array
    //TODO: Curretly doesn't remove objects that leave onTriggerExit
    private void OnTriggerEnter(Collider other)
    {
        ObjClass obj = other.gameObject.GetComponentInChildren<ObjClass>();

        if (obj)
        {
            Debug.Log("new item in scoring area: " + other.gameObject.name);
            //CalculateScore();
            //Debug.Log("Object added to score: " + other.gameObject.name);

            if (objectsToScore.Contains(new ItemRequirement(obj, 1)))
            {
                int foundIdx = objectsToScore.FindIndex(requirement => requirement.Equals(obj));
                objectsToScore[foundIdx].quantity += 1;
            }
            else
            {
                objectsToScore.Add(new ItemRequirement(obj, 1));
                Debug.Log("new requirement added, total # of objs now " + objectsToScore.Count);
            }
        }
    }
}
