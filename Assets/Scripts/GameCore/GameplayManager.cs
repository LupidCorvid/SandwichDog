using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameplayManager : Singleton<GameplayManager>
{
    public const int maxLevels = 2; //How many levels the game has
    public int currentLevel;

    public GameLevel_SO gameLevel;
    public bool gameOver;

    private void Awake()
    {
        gameOver = false;
        currentLevel = gameLevel.levelNumber;
    }

    void Start()
    {
        //If the amount of tutorials is greater than the current level, then there's a tutorial to proc
        if (TutorialManager.Instance.levelTutorialObjects.Length >= currentLevel && gameLevel.hasTutorialInfo) TutorialManager.Instance.startTutorial(currentLevel);

        //Set all other objects
        //PrepareLevel();
    }

    GameplayManager()
    {
        EditorApplication.playModeStateChanged += OnPlayStateExited;
    }

    //Since the SO edit is a permanent change, it needs to reset when unity playmode is exited
    public void OnPlayStateExited(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            ResetSO();
        }
    }

    //Call on scene start
    public void PrepareLevel()
    {
        SpawnLevelObjects();
    }

    private void SpawnLevelObjects()
    {
        foreach (ObjectSpawner spawner in gameLevel.levelObjects.objectAssignments)
        {
            GameObject spawnedObject = Instantiate(spawner.prefabToSpawn, spawner.positionToSpawn, spawner.rotationToSpawn);

            spawnedObject.transform.localScale = spawner.scaleToSpawn;
        }
    }
    
    //Call on scene end, when player clicks next level button
    public void IncrementLevel()
    {
        if (gameLevel.nextLevel)
        {
            gameLevel.levelNumber = gameLevel.nextLevel.levelNumber;
            gameLevel.hasTutorialInfo = gameLevel.nextLevel.hasTutorialInfo;
            gameLevel.levelObjects = gameLevel.nextLevel.levelObjects;
            gameLevel.levelRecipe = gameLevel.nextLevel.levelRecipe;
            gameLevel.nextLevel = gameLevel.nextLevel.nextLevel == null ? gameLevel.nextLevel.nextLevel : null;
            ResetLevel();
        }
        else
        {
            ResetSO();
            SceneManager.LoadScene("MainMenu");
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

    public void ResetSO()
    {
        gameLevel.levelNumber = gameLevel.firstLevel.levelNumber;
        gameLevel.hasTutorialInfo = gameLevel.firstLevel.hasTutorialInfo;
        gameLevel.levelObjects = gameLevel.firstLevel.levelObjects;
        gameLevel.levelRecipe = gameLevel.firstLevel.levelRecipe;
        gameLevel.nextLevel = gameLevel.firstLevel.nextLevel == null ? gameLevel.nextLevel.nextLevel : null;
    }

    public void SwapOutObj(GameObject objToDelete, GameObject objToSpawn)
    {
        Vector3 position = objToDelete.transform.position;
        Quaternion rotation = objToDelete.transform.rotation;
        Transform parent = objToDelete.transform.parent;

        Destroy(objToDelete);
        Instantiate(objToSpawn, position, rotation, parent);
    }
}
