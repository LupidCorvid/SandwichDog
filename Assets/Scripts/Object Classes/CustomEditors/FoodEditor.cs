using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Food))]
public class FoodEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Food myScript = (Food)target;
        if (GUILayout.Button("Assign Starting Postion"))
        {
            myScript.setStartingPos();
        }
    }
}
