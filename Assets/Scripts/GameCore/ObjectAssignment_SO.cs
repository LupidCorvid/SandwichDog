using UnityEngine;

[System.Serializable]
public struct ObjectSpawner
{
    public Vector3 positionToSpawn;
    public Quaternion rotationToSpawn;
    public Vector3 scaleToSpawn;
    public GameObject prefabToSpawn;

    public ObjectSpawner(Transform inTransform, GameObject inGameObject)
    {
        positionToSpawn = inTransform.position;
        rotationToSpawn = inTransform.rotation;
        scaleToSpawn = inTransform.localScale;
        prefabToSpawn = inGameObject;
        //Debug.Log("new transform data: " + positionToSpawn + rotationToSpawn + scaleToSpawn);
    }
}

[CreateAssetMenu(fileName = "ObjectAssignment_SO", menuName = "Scriptable Objects/ObjectAssignment_SO")]
public class ObjectAssignment_SO : ScriptableObject
{
    public ObjectSpawner[] objectAssignments;
}
