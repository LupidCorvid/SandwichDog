using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecipeScorer : MonoBehaviour
{
    private const float MAX_SCORE_DRAIN_NO_MATCH = 0.2f;
    private const float PENALTY_PER_EXTRA_OBJ = 0.05f;

    public float waitTimeBeforeScoring; //Time to wait in seconds for the player to stand in the box before scoring objects
    private float timer;
    private bool scoreCalculated;

    public static event Action onScoreCalculate;

    //The objects the player brought to the end game area, added via end area OnTriggerEnter
    private List<Food> foodsToScore = new List<Food>();
    private List<Sandwich> sandwiches = new List<Sandwich>(); // TODO(?) HACK way to store list of sandwiches in end zone
    private List<FoodRequirement> recipeRequirements;

    public TMP_Text recipeToMakeText;
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

        if (GameplayManager.Instance.currentLevel == 1) recipeToMakeText.text = "Make a PBJ!";
        else if (GameplayManager.Instance.currentLevel == 2) recipeToMakeText.text = "Make a Grilled Cheese!";
        else if (GameplayManager.Instance.currentLevel == 3) recipeToMakeText.text = "Make a Chicken Sandwich!";

    }

    private void OnEnable()
    {
        onScoreCalculate += CalculateScore;
    }

    private void OnTriggerEnter(Collider other)
    {
        Food targetFood = other.GetComponent<Food>();

        if (!targetFood) return;

        // TODO(?) - WON'T WORK FOR PARENT HIERARCHIES WITH NON-FOOD INBETWEEN
        while (targetFood && targetFood.objOwner)
        {
            targetFood = targetFood.objOwner as Food;
        }

        Sandwich targetSandwich = targetFood as Sandwich;
        if (targetSandwich != null)
        {
            if (!sandwiches.Any(sandwich => ReferenceEquals(targetSandwich, sandwich)))
            {
                foreach (Food food in targetSandwich.foodOrder)
                {
                    Debug.Log(other.name + " HAS ENTERED END ZONE VIA SANDWICH");
                    foodsToScore.Add(food);
                }
                sandwiches.Add(targetSandwich);
            }
            return;
        }
        else if (targetFood)
        {
            Debug.Log(other.name + " HAS ENTERED END ZONE");
            foodsToScore.Add(targetFood);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Food targetFood = other.gameObject.GetComponentInChildren<Food>();

        if (!targetFood) return;

        while (targetFood && targetFood.objOwner)
        {
            targetFood = targetFood.objOwner as Food;
        }

        Sandwich targetSandwich = targetFood as Sandwich;
        if (targetSandwich != null)
        {
            sandwiches.Remove(targetSandwich);
            return;
        }
        else if (targetFood)
        {
            Debug.Log(other.name + " HAS LEFT THE END ZONE");
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

        float scoreRaw = 0.0f;
        float totalWeight = 0.0f;
        float scorePercent = 0.0f;

        // get all best matches first
        FoodRequirement currReq;
        Food foodToScore;
        int currRecipeIdx = 0;
        float testScore, bestScore;
        int reqToRemoveIdx = 0;
        int foodToRemoveIdx = 0;

        while (currRecipeIdx < recipeRequirements.Count)
        {
            Debug.Log("our curr recipe reqs are:");
            for (int i = 0; i < recipeRequirements.Count; i++)
            {
                Debug.Log(recipeRequirements[i].food.name);
            }
            Debug.Log("our curr foods to score are:");
            for (int i = 0; i < foodsToScore.Count; i++)
            {
                Debug.Log(foodsToScore[i].name);
            }
            Debug.Log("our curr recipe idx is: " + currRecipeIdx);

            currReq = recipeRequirements[currRecipeIdx];
            bestScore = 0;

            // start from 1st food for curr req
            foodToRemoveIdx = -1;
            foodToScore = null;

            // check all foods against the curr req for the best match
            // recipe req stable (nonchanging), food unstable (changing)
            for (int i = 0; i < foodsToScore.Count; i++)
            {
                foodToScore = foodsToScore[i];

                if (currReq.food.Equals(foodToScore))
                {
                    testScore = foodToScore.ScoreFood(currReq);

                    if (testScore > bestScore)
                    {
                        bestScore = testScore;
                        foodToRemoveIdx = i;
                    }
                }
            }
            // can only score if there was some match for the req
            if (foodToRemoveIdx >= 0)
            {
                foodToScore = foodsToScore[foodToRemoveIdx];
                reqToRemoveIdx = currRecipeIdx;

                // check if that food satisfies another req better
                // food stable, recipe req unstable
                for (int i = 0; i < recipeRequirements.Count; i++)
                {
                    if (recipeRequirements[i].food.Equals(foodToScore))
                    {
                        testScore = foodToScore.ScoreFood(recipeRequirements[i]);

                        // if so, then remove *that* req with the curr food instead
                        if (testScore > bestScore)
                        {
                            bestScore = testScore;
                            reqToRemoveIdx = i;
                        }
                    }
                }

                // remove whichever req-food pair was decided on and add its score
                scoreRaw += bestScore;
                totalWeight += recipeRequirements[reqToRemoveIdx].food.FoodWeight;

                Debug.Log(recipeRequirements[reqToRemoveIdx].food.name + 
                    " was judged as the best req for " + foodsToScore[foodToRemoveIdx].name +
                    ", with a score of " + bestScore +
                    " and weight of " + recipeRequirements[reqToRemoveIdx].food.FoodWeight);

                recipeRequirements.RemoveAt(reqToRemoveIdx);
                foodsToScore.RemoveAt(foodToRemoveIdx);
                // account for going back if curr recipe was used in the match
                if (reqToRemoveIdx == currRecipeIdx && currRecipeIdx > 0)
                {
                    currRecipeIdx--;
                }
                // next loop will go again with whichever recipe is now at the head of the req lis
            }
            // if no food matches, go to next reqs
            else
            {
                currRecipeIdx++;
            }
        }
        
        // all recipe reqs that are left unmeet also contribute to a % loss of total score by including themselves in the total weight
        foreach (FoodRequirement req in recipeRequirements)
        {
            totalWeight += req.food.FoodWeight;
        }

        // all extra foods that meet no reqs contribute to a negative % loss of total score
        float nonMatchPenalty = 0.0f;

        for (int i = 0; i < foodsToScore.Count; i++)
        {
            nonMatchPenalty += PENALTY_PER_EXTRA_OBJ;

            if (nonMatchPenalty >= MAX_SCORE_DRAIN_NO_MATCH)
            {
                nonMatchPenalty = MAX_SCORE_DRAIN_NO_MATCH;
                break;
            }
        }
        // apply penalty for 

        // get base score from all met reqs
        if (totalWeight > 0.0f)
        {
            scorePercent = scoreRaw / totalWeight;
            scorePercent = Mathf.Clamp(scorePercent - nonMatchPenalty, 0.0f, 1.0f);
            scorePercent *= 100.0f;
        }

        //Debug.Log("total weight in end area is " + totalWeight);

        //Update score text
        //Debug.Log("Score:" + score);
        scoreText.text = scorePercent.ToString("F2").Truncate(5) + "%";

        foreach (GameObject button in postScoreButtons)
        {
            button.SetActive(true);
        }
    }
}
