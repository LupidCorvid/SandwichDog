using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjClass))]
public class ObjClassEditor : Editor
{
    SerializedProperty overrideProperty;
    SerializedProperty canGetDirtyProperty;
    SerializedProperty dirtMaterialProperty;
    SerializedProperty amountToDirtyProperty;
    SerializedProperty canGetCleanProperty;
    SerializedProperty amountToCleanProperty;
    SerializedProperty objSettingsProperty;

    protected virtual void OnEnable()
    {
        AssignEditorSettings();

        objSettingsProperty = serializedObject.FindProperty("objSettings");
        overrideProperty = serializedObject.FindProperty("overrideGlobalSettings");
        dirtMaterialProperty = serializedObject.FindProperty("dirtMaterial");
        canGetDirtyProperty = serializedObject.FindProperty("canGetDirty");
        amountToDirtyProperty = serializedObject.FindProperty("amountToDirtyPerSecond");
        canGetCleanProperty = serializedObject.FindProperty("canGetClean");
        amountToCleanProperty = serializedObject.FindProperty("amountToCleanPerSecond");
    }

    public override void OnInspectorGUI()
    {
        ObjClass targetObj = target as ObjClass;
        if (!targetObj) return;

        serializedObject.Update();
        EditorGUILayout.PropertyField(objSettingsProperty);
        EditorGUILayout.PropertyField(overrideProperty);

        if (overrideProperty.boolValue)
        {
            DrawProperties();
        }
        else
        {
            targetObj.InitializeSettings();
        }

            serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawProperties()
    {
        EditorGUILayout.PropertyField(canGetDirtyProperty);
        if (canGetDirtyProperty.boolValue)
        {
            EditorGUILayout.PropertyField(dirtMaterialProperty);
            EditorGUILayout.PropertyField(amountToDirtyProperty);
        }

        EditorGUILayout.PropertyField(canGetCleanProperty);
        if (canGetCleanProperty.boolValue)
        {
            EditorGUILayout.PropertyField(amountToCleanProperty);
        }
    }

    private void AssignEditorSettings()
    {
        ObjClass targetObj = (ObjClass)target;

        if (!targetObj.objSettings)
        {
            GlobalObjSettings_SO loadSettings = Resources.Load<GlobalObjSettings_SO>(GlobalObjSettings_SO.GetPath());

            targetObj.objSettings = loadSettings;
        }
    }
}

[CustomEditor(typeof(Food))]
public class FoodClassEditor : ObjClassEditor
{
    SerializedProperty isCookableProperty;
    SerializedProperty timeToCookProperty;
    SerializedProperty timeToBurnProperty;
    SerializedProperty cookedColorProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        isCookableProperty = serializedObject.FindProperty("isCookable");
        timeToCookProperty = serializedObject.FindProperty("timeToCook");
        timeToBurnProperty = serializedObject.FindProperty("timeToBurn");
        cookedColorProperty = serializedObject.FindProperty("cookedColor");
    }

    protected override void DrawProperties()
    {
        base.DrawProperties();

        EditorGUILayout.PropertyField(isCookableProperty);
        if (isCookableProperty.boolValue)
        {
            EditorGUILayout.PropertyField(timeToCookProperty);
            EditorGUILayout.PropertyField(timeToBurnProperty);
            EditorGUILayout.PropertyField(cookedColorProperty);
        }
    }
}
