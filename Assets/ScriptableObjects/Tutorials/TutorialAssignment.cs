using UnityEngine;

[System.Serializable]
public struct TutorialObj
{
    public Vector3 arrowPositionToSpawn;
    public Vector3 UIPositionToSpawn;
    public Vector3 UIRotationToSpawn;
    public GameObject arrow;
    public GameObject instructionUI;
    public Sprite controllerImg1;
    public Sprite controllerImg2;
    public string UIText;

    public TutorialObj(Transform arrow_inTransform, GameObject arrow_inGameObject, Vector3 UI_inPosition, Vector3 UI_inRotation, GameObject UI_inGameObject, Sprite in_img1, Sprite in_img2, string inText)
    {
        arrowPositionToSpawn = arrow_inTransform.position;
        arrow = arrow_inGameObject;
        UIPositionToSpawn = UI_inPosition;
        UIRotationToSpawn = UI_inRotation;
        instructionUI = UI_inGameObject;
        UIText = inText;
        controllerImg1 = in_img1;
        controllerImg2 = in_img2;
        //Debug.Log("new transform data: " + positionToSpawn + rotationToSpawn + scaleToSpawn);
    }
}


[CreateAssetMenu(fileName = "TutorialAssignment", menuName = "Scriptable Objects/TutorialAssignment")]
public class TutorialAssignment : ScriptableObject
{
    public TutorialObj [] assignedTutorialObjs;
}
