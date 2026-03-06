using UnityEditor;
using UnityEngine;

/*
 Details about this script:

The goal of the object manager is to manage objects for the game.

At the start of the game, it should read from a public array of 
ObjClass objects and spawn them in the scene. Where to spawn 
them is stored in the Vector3 variable ObjClass.startingPosition. 
It's already set to 0,0,0, so make sure to set it to the worldspace location
that the object should spawn in.
 
The way the public array works, is you set the array initialization here, 
then add the prefabs to the ObjectManager script in the inspector window

 */

public class ObjectManager : MonoBehaviour
{

public ObjectAssignment_SO levelObjects;

    void Start()
    {
        foreach (ObjectSpawner spawner in levelObjects.objectAssignments)
        {
            GameObject spawnedObject = Instantiate(spawner.prefabToSpawn);

            spawnedObject.transform.position = spawner.positionToSpawn;
            spawnedObject.transform.rotation = spawner.rotationToSpawn;
            spawnedObject.transform.localScale = spawner.scaleToSpawn;
        }
    }
}
