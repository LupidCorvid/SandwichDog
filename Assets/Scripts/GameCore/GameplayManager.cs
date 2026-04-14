using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayManager : Singleton<GameplayManager>
{
    public const int maxLevels = 2; //How many levels the game has
    public static int currentLevel = 1; //The current level the player is playing //TODO: find a better way to do this

    public Recipe_SO [] levelRecipe;
    public bool gameOver;

    private void Awake()
    {
        gameOver = false;
    }

    void Start()
    {
        if (currentLevel == 1) TutorialManager.Instance.startTutorial(currentLevel);
        PrepareLevel();
    }

    void Update()
    {
        //print(currentLevel);
    }

    //Call on scene end, when player clicks next level button
    public void IncrementLevel()
    {
        currentLevel++;
        if (currentLevel > maxLevels)
        {
            currentLevel = 1;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("GameRoom");
        }
    } 

    public void ResetLevel()
    {
        SceneManager.LoadScene("GameRoom");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Call on scene start
    public void PrepareLevel()
    {
        
    }
}
