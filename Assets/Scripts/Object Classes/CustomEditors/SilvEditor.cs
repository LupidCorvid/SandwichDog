using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Silverware))]
public class SilvEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Silverware myScript = (Silverware)target;
        if (GUILayout.Button("Assign Starting Postion"))
        {
            myScript.setStartingPos();
        }
    }
}

