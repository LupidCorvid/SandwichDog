using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjClass))]
public class ObjClassEditor : Editor
{
    SerializedProperty objSettingsProperty;
    SerializedProperty overrideProperty;

    SerializedProperty canHaveSpreadsProperty;
    SerializedProperty possibleSpreadsProperty;

    SerializedProperty canGetDirtyProperty;
    SerializedProperty dirtMaterialProperty;
    SerializedProperty amountToDirtyProperty;
    SerializedProperty canGetCleanProperty;
    SerializedProperty amountToCleanProperty;

    protected virtual void OnEnable()
    {
        AssignEditorSettings();

        canHaveSpreadsProperty = serializedObject.FindProperty("canHaveSpreads");
        
        possibleSpreadsProperty = serializedObject.FindProperty("possibleSpreads");

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

        if (!targetObj.objSettings || !overrideProperty.boolValue)
        {
            targetObj.InitializeSettings();
        }
        else
        {
            DrawProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawProperties()
    {
        EditorGUILayout.PropertyField(canHaveSpreadsProperty);
        if (canHaveSpreadsProperty.boolValue)
        {
            EditorGUILayout.PropertyField(possibleSpreadsProperty);
        }

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
            string loadPath = GlobalObjSettings_SO.DefaultObjPath;

            if (targetObj as Food)
            {
                loadPath = GlobalObjSettings_SO.FoodObjPath;
            }
            else if (targetObj as Silverware)
            {
                loadPath = GlobalObjSettings_SO.SilverwareObjPath;
            }

            GlobalObjSettings_SO loadSettings = Resources.Load<GlobalObjSettings_SO>(loadPath);
            targetObj.objSettings = loadSettings;
        }
    }

    [CustomEditor(typeof(Food))]
    public class FoodClassEditor : ObjClassEditor
    {
        // cooking
        SerializedProperty isCookableProperty;
        SerializedProperty timeToCookProperty;
        SerializedProperty timeToBurnProperty;
        SerializedProperty cookedColorProperty;
        
        // stacking
        SerializedProperty isStackableProperty;
        SerializedProperty topStackSnapPointProperty;
        //SerializedProperty debugFoodToSnapToProperty;

        // making slices
        SerializedProperty isSliceableProperty;
        SerializedProperty numCutsNeededProperty;
        SerializedProperty slicedResultObjectProperty;
        
        SerializedProperty wasSlicedProperty;
        SerializedProperty sliceSourceProperty;
        

        protected override void OnEnable()
        {
            base.OnEnable();
            isCookableProperty = serializedObject.FindProperty("isCookable");
            timeToCookProperty = serializedObject.FindProperty("timeToCook");
            timeToBurnProperty = serializedObject.FindProperty("timeToBurn");
            cookedColorProperty = serializedObject.FindProperty("cookedColor");
            
            isStackableProperty = serializedObject.FindProperty("isStackable");
            topStackSnapPointProperty = serializedObject.FindProperty("topStackSnapPoint");

            isSliceableProperty = serializedObject.FindProperty("isSliceable");
            numCutsNeededProperty = serializedObject.FindProperty("numCutsNeeded");
            slicedResultObjectProperty = serializedObject.FindProperty("slicedResultObject");
                        
            wasSlicedProperty = serializedObject.FindProperty("wasSliced");
            sliceSourceProperty = serializedObject.FindProperty("sliceSource");
            
            //debugFoodToSnapToProperty= serializedObject.FindProperty("debugFoodToSnapTo");
        }

        protected override void DrawProperties()
        {
            Food foodTarget = target as Food;
            if (!foodTarget) return; 

            base.DrawProperties();

            EditorGUILayout.PropertyField(isCookableProperty);
            if (isCookableProperty.boolValue)
            {
                EditorGUILayout.PropertyField(timeToCookProperty);
                EditorGUILayout.PropertyField(timeToBurnProperty);
                EditorGUILayout.PropertyField(cookedColorProperty);
            }

            EditorGUILayout.PropertyField(isStackableProperty);
            if (isStackableProperty.boolValue)
            {
                if (!foodTarget.topStackSnapPoint)
                {
                    foodTarget.topStackSnapPoint = new GameObject(foodTarget.name + "TopPoint").transform;
                    foodTarget.topStackSnapPoint.SetParent(foodTarget.transform, false);

                    topStackSnapPointProperty.objectReferenceValue = foodTarget.topStackSnapPoint;
                }
            }

            EditorGUILayout.PropertyField(isSliceableProperty);
            if (isSliceableProperty.boolValue)
            {
                EditorGUILayout.PropertyField(numCutsNeededProperty);
                EditorGUILayout.PropertyField(slicedResultObjectProperty);
            }
            EditorGUILayout.PropertyField(wasSlicedProperty);
            if (wasSlicedProperty.boolValue)
            {
                EditorGUILayout.PropertyField(sliceSourceProperty);
            }

            //EditorGUILayout.PropertyField(debugFoodToSnapToProperty);
            //if (GUILayout.Button("Stack Food"))
            //{
                //foodTarget.SnapTo(foodTarget.debugFoodToSnapTo);
            //}
        }
    }

    [CustomEditor(typeof(Silverware))]
    public class SilverwareClassEditor : ObjClassEditor
    {
        SerializedProperty isSlicerProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            isSlicerProperty = serializedObject.FindProperty("isSlicer");
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();

            EditorGUILayout.PropertyField(isSlicerProperty);
        }
    }
}