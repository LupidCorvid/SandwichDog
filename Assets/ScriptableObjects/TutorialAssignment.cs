using UnityEngine;

[System.Serializable]
public struct TutorialObj
{
    public Vector3 arrowPositionToSpawn;
    public Vector3 UIPositionToSpawn;
    public GameObject arrow;
    public GameObject instructionUI;
    public string UIText;

    public TutorialObj(Transform arrow_inTransform, GameObject arrow_inGameObject, Transform UI_inTransform, GameObject UI_inGameObject, string inText)
    {
        arrowPositionToSpawn = arrow_inTransform.position;
        arrow = arrow_inGameObject;
        UIPositionToSpawn = UI_inTransform.position;
        instructionUI = UI_inGameObject;
        UIText = inText;
        //Debug.Log("new transform data: " + positionToSpawn + rotationToSpawn + scaleToSpawn);
    }
}


[CreateAssetMenu(fileName = "TutorialAssignment", menuName = "Scriptable Objects/TutorialAssignment")]
public class TutorialAssignment : ScriptableObject
{
    public TutorialObj [] assignedTutorialObjs;
}
