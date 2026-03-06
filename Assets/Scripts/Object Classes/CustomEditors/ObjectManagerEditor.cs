using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ObjectManager))]
public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Save All Scene Objs to SO"))
        {
            AddAllSceneObjectsToSpawnerSO();
        }

        ObjClass obj = target as ObjClass;

        if (!obj)
        {
            return;
        }

        if (obj.transform.hasChanged)
        {
            AddAllSceneObjectsToSpawnerSO();
        }

        void AddAllSceneObjectsToSpawnerSO()
        {
            ObjectManager objectManager = FindFirstObjectByType<ObjectManager>(FindObjectsInactive.Exclude);

            if (!objectManager)
            {
                return;
            }
            if (!objectManager.levelObjects)
            {
                return;
            }
            Debug.Log("Saving scene object spawn layout to " + objectManager.levelObjects.name + " ...");

            ObjClass[] sceneObjects = FindObjectsByType<ObjClass>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            List<ObjectSpawner> newAssignments = new List<ObjectSpawner>();
            List<ObjClass> addedObjects = new List<ObjClass>();
            // prune all GOs that are nested objects (e.g jar having lid + jar body)
            foreach (ObjClass sceneObject in sceneObjects)
            {
                // we only need to the top obj's GO, as it *should* be our prefab to spawn
                Transform currObj = sceneObject.gameObject.transform;
                ObjClass topObj = sceneObject;

                while (currObj.gameObject.transform.parent)
                {
                    currObj = currObj.transform.parent;

                    ObjClass newTopObj = currObj.gameObject.GetComponent<ObjClass>();

                    if (newTopObj)
                    {
                        topObj = newTopObj;
                    }
                }

                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(topObj.gameObject);

                if (!addedObjects.Contains(topObj))
                {
                    addedObjects.Add(topObj);
                    newAssignments.Add(new ObjectSpawner(topObj.gameObject.transform, prefab));
                    Debug.Log("Adding object " + prefab.gameObject.name + " at location " + topObj.gameObject.transform.position);
                }
            }

            objectManager.levelObjects.objectAssignments = newAssignments.ToArray();
        }
    }
}
