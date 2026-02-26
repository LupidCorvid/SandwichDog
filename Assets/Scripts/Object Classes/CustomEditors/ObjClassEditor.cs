using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjClass))]
public class ObjClassEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjClass myScript = (ObjClass)target;
        if (GUILayout.Button("Assign Starting Postion"))
        {
            myScript.setStartingPos();
        }
    }
}
