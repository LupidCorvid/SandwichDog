using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeScorer : MonoBehaviour
{
    public float waitTimeBeforeScoring; //Time to wait in seconds for the player to stand in the box before scoring objects
    private float timer;
    private bool scoreCalculated;

    public static event Action onScoreCalculate;

    //The objects the player brought to the end game area, added via end area OnTriggerEnter
    private List<Food> foodsToScore = new List<Food>();
    private List<FoodRequirement> recipeRequirements;

    public TMP_Text displayText;
    public TMP_Text scoreText;

    public GameObject scoreButton;
    public GameObject[] postScoreButtons;

    private void Awake()
    {
        scoreCalculated = false;
        displayText.text = "";
        scoreText.text = "";

        timer = waitTimeBeforeScoring;

        foreach (GameObject button in postScoreButtons)
        {
            button.SetActive(false);
        }
    }

    private void Start()
    {
        recipeRequirements = new List<FoodRequirement>(GameplayManager.Instance.gameLevel.levelRecipe.requiredFood);
    }

    private void OnEnable()
    {
        onScoreCalculate += CalculateScore;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " HAS ENTERED END ZONE");

        // ugly but needed for URCAD
        Food targetFood = other.GetComponentInParent<Sandwich>();
        if (!targetFood) targetFood = other.gameObject.GetComponentInChildren<Food>();

        if (!targetFood) return;

        if (targetFood && !targetFood.foodParent)
        {
            Debug.Log(targetFood.name + " HAS ENTERED?");
            if (!foodsToScore.Contains(targetFood))
            {
                foodsToScore.Add(targetFood);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Food targetFood = other.gameObject.GetComponentInChildren<Food>();

        if (targetFood && !targetFood.foodParent)
        {
            foodsToScore.Remove(targetFood);
        }

        //If the player leaves too early, reset the timer
        if (other.gameObject.CompareTag("Player") && timer > 0)
        {
            timer = waitTimeBeforeScoring;
            displayText.text = "";
        }
    }

    //When the player enters, wait for 5 seconds and then score
    private void OnTriggerStay(Collider other)
    {
        //DEBUG checks if the plate entered the scoring layer
        //Change it to let the player click a UI element to start the scoring
        //if (other.gameObject.CompareTag("Player")) //if (other.gameObject.CompareTag("Player")) //if (other.gameObject.layer == 10)
        //{
        //    if (timer > 0) timer -= Time.deltaTime;

        //    if (timer > (waitTimeBeforeScoring * 0.75)) displayText.text = "Hold still";
        //    else if (timer > (waitTimeBeforeScoring * 0.5)) displayText.text = "Hold still.";
        //    else if (timer > (waitTimeBeforeScoring * 0.25)) displayText.text = "Hold still..";
        //    else if (timer > 0) displayText.text = "Hold still...";
        //    else if (timer <= 0 && !scoreCalculated)
        //    {
        //        onScoreCalculate?.Invoke();
        //        scoreCalculated = true;
        //    }
        //}
    }

    public void CalculateScore()
    {
        displayText.text = "Your Score: ";
        scoreCalculated = true;
        scoreButton.SetActive(false);

        float score = 0.0f;
        float totalWeight = 0.0f;
        Food foodToScore;

        Debug.Log("SCORE TIME");
        while (foodsToScore.Count > 0)
        {
            Debug.Log("SCORING...");
            foodToScore = foodsToScore[0];
            totalWeight += foodToScore.GetFoodWeight();
            Debug.Log("Food to score:" + foodToScore.name + " with weight " + foodToScore.GetFoodWeight());

            score += foodToScore.AttemptScoreFood(recipeRequirements);

            //Debug.Log(recipeRequirements.Count);
            //Debug.Log(recipeRequirements.Count);

            foodsToScore.RemoveAt(0);
        }
        Debug.Log("still " + recipeRequirements.Count + " food from recipe not in the end area!");
        totalWeight += recipeRequirements.Count;

        Debug.Log("total score of " + score + " vs weight of " + totalWeight);
        score = (score / totalWeight) * 100.0f;

        //Update score text
        Debug.Log("Score:" + score);
        scoreText.text = score.ToString("F2").Truncate(5) + "%";

        foreach (GameObject button in postScoreButtons)
        {
            button.SetActive(true);
        }
    }
}
