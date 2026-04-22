using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeScorer : MonoBehaviour
{
    public float waitTimeBeforeScoring; //Time to wait in seconds for the player to stand in the box before scoring objects
    private float timer;
    private float score = 0;
    private bool scoreCalculated;

    public static event Action onScoreCalculate;

    //The objects the player brought to the end game area, added via end area OnTriggerEnter
    private List<Food> foodsToScore = new List<Food>();
    private List<Food> recipeRequirements;

    public TMP_Text scoreText;
    public TMP_Text displayText;

    private void Awake()
    {
        scoreCalculated = false;
        displayText.text = "0%";

        recipeRequirements = new List<Food>(GameplayManager.Instance.levelRecipe.requiredFood);
        timer = waitTimeBeforeScoring;
    }

    private void OnEnable()
    {
        onScoreCalculate += CalculateScore;
    }

    //Add objects to the objectsToScore array
    //TODO: Curretly doesn't remove objects that leave onTriggerExit
    private void OnTriggerEnter(Collider other)
    {
        Food targetFood = other.gameObject.GetComponentInChildren<Food>();

        if (targetFood)
        {
            foodsToScore.Add(targetFood);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Food targetFood = other.gameObject.GetComponentInChildren<Food>();

        if (targetFood)
        {
            foodsToScore.Remove(targetFood);
        }

        //If the player leaves too early, reset the timer
        if (other.gameObject.CompareTag("Player") && timer > 0)
        {
            timer = waitTimeBeforeScoring;
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

            if (timer <= 0 && !scoreCalculated)
            {
                onScoreCalculate?.Invoke();
                scoreCalculated = true;
            }
        }
    }

    public void CalculateScore()
    {
        float score = 0.0f;
        float totalWeight = 0.0f;
        Food foodToScore = null;

        while (foodsToScore.Count > 0)
        {

            foodToScore = foodsToScore[0];
            totalWeight += foodToScore.GetFoodWeight();

            foodToScore = foodToScore.AttemptRemoveFoodFromRecipe(recipeRequirements);
            // only applies pos influence if present in recipe            {
            if (foodToScore)
            {
                score += foodToScore.ScoreFood();
            }

            foodsToScore.RemoveAt(0);
        }
        Debug.Log("still " + recipeRequirements.Count + " food from recipe not in the end area!");
        totalWeight += recipeRequirements.Count;

        score = (score / totalWeight) * 100.0f;

        //Update score text
        Debug.Log("Score:" + score);
        displayText.text = score.ToString("F2").Truncate(5) + "%";
    }
}
