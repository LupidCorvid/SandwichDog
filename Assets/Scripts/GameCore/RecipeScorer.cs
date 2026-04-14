using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeScorer : MonoBehaviour
{
    public float timer; //Time to wait in seconds for the player to stand in the box before scoring objects
    public float scoreMax = 0.0f;
    public float score = 0;
    public bool gameOver;

    public static event Action onScoreCalculate;

    //The objects the player brought to the end game area, added via end area OnTriggerEnter
    public List<ItemRequirement> objectsToScore = new List<ItemRequirement>();

    public TMP_Text scoreText;
    public TMP_Text displayText;

    private void Awake()
    {
        gameOver = false;
        displayText.text = "0%";
        scoreMax = levelRecipe[currentLevel - 1].requirements.Length;
    }

    private void OnEnable()
    {
        onScoreCalculate += CalculateScore;
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

    //If the player leaves too early, reset the timer
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("removing item in scoring area: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player") && timer > 0)
        {
            timer = 5;
            displayText.text = "0%";
        }
    }

    //When the player enters, wait for 5 seconds and then score
    private void OnTriggerStay(Collider other)
    {
        //DEBUG checks if the plate entered the scoring layer
        //Change it to let the player click a UI element to start the scoring
        if (other.gameObject.layer == 10) //if (other.gameObject.CompareTag("Player")) //if (other.gameObject.layer == 10)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                onScoreCalculate?.Invoke();
            }
        }
    }

    public void CalculateScore()
    {
        List<ItemRequirement> itemRequirements = levelRecipe[currentLevel - 1].requirements.ToList();

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
    }
}
