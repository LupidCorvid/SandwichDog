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
    public ObjClass[] objects;

    void Start()
    {
        objects = FindObjectsByType<ObjClass>(FindObjectsSortMode.None);
        foreach (ObjClass obj in objects)
        {
            SetLocation(obj);
        }
    }
    
    void SetLocation(ObjClass obj)
    {
        obj.transform.position = obj.startingPosition;
        Debug.Log("Object " + obj.name + " spawned at " + obj.startingPosition);
    }
}
