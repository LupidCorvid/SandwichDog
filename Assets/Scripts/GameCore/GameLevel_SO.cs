using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel_SO", menuName = "Scriptable Objects/GameLevel_SO")]
public class GameLevel_SO : ScriptableObject
{
    [SerializeField] public int levelNumber;
    [SerializeField] public bool hasTutorialInfo;
    [SerializeField] public ObjectAssignment_SO levelObjects;
    [SerializeField] public Recipe_SO levelRecipe;
    [SerializeField] public GameLevel_SO nextLevel;
    [SerializeField] public GameLevel_SO firstLevel;
}
