using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Jar))]
public class JarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Jar myScript = (Jar)target;
        if (GUILayout.Button("Assign Starting Postion"))
        {
            myScript.setStartingPos();
        }
    }
}
