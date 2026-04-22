using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecipeScorer))]
public class RecipeScorererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RecipeScorer scorer = (RecipeScorer)target;

        if (GUILayout.Button("Score Food"))
        {
            scorer.CalculateScore();
        }

        base.OnInspectorGUI();
    }
}
