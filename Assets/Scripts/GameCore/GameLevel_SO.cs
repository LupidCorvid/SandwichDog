using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel_SO", menuName = "Scriptable Objects/GameLevel_SO")]
public class GameLevel_SO : ScriptableObject
{
    public bool isTutorial;
    public Recipe_SO levelRecipe;
    public GameLevel_SO nextLevel;
}
