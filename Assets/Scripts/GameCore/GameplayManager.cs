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
    public int currentLevel = 1; //The current level the player is playing //TODO: find a better way to do this

    public GameLevel_SO gameLevel;
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
            gameLevel = gameLevel.nextLevel;
            ResetLevel();
        }
        else
        {
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

    public void SwapOutObj(GameObject objToDelete, GameObject objToSpawn)
    {
        Vector3 position = objToDelete.transform.position;
        Quaternion rotation = objToDelete.transform.rotation;
        Transform parent = objToDelete.transform.parent;

        Destroy(objToDelete);
        Instantiate(objToSpawn, position, rotation, parent);
    }
}
