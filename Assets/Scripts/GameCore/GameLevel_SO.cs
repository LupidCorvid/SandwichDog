using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel_SO", menuName = "Scriptable Objects/GameLevel_SO")]
public class GameLevel_SO : ScriptableObject
{
    public int levelNumber;
    public bool hasTutorialInfo;
    public ObjectAssignment_SO levelObjects;
    public Recipe_SO levelRecipe;
    public GameLevel_SO nextLevel;
}
