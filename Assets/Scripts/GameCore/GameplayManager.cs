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
    public GameLevel_SO currentLevel;

    public static event Action<ObjClass> OnRecipeProgressUpdated;
    public static event Action OnTest;

    void Start()
    {
        if (currentLevel.isTutorial) TutorialManager.Instance.startTutorial(currentLevel);
        PrepareLevel();
    }

    void Update()
    {
        //print(currentLevel);
    }

    //Call on scene end, when player clicks next level button
    public void IncrementLevel()
    {
        if (currentLevel.nextLevel)
        {
            currentLevel = currentLevel.nextLevel;
            ResetLevel();
        }
        else
        {
            GoToMainMenu();
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
        if (clipBoardTextGO != null)
        {
            clipboardText.GetComponent<TMPro.TextMeshProUGUI>().text = clipboardText.assignedRecipes[currentLevel - 1].combinedText;
        }
    }
}
