using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(China))]
public class ChinaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        China myScript = (China)target;
        if (GUILayout.Button("Assign Starting Postion"))
        {
            myScript.setStartingPos();
        }
    }
}


