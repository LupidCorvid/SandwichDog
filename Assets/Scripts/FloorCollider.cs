using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

[SerializeField]
public struct DirtStopwatch
{
    public ObjClass objectToDirty;
    private float startTime;

    public DirtStopwatch(ObjClass inObject, float inStartTime)
    {
        objectToDirty = inObject;
        startTime = inStartTime;
    }

    public float GetTimePassed()
    {
        return Time.deltaTime - startTime;
    }
}

public class FloorCollider : MonoBehaviour
{
    private List<DirtStopwatch> dirtStopwatches = new List<DirtStopwatch>();
    public float amountToDirtyPerSecond;

    private void OnTriggerEnter(Collider other)
    {
        ObjClass obj = other.gameObject.GetComponentInChildren<ObjClass>();
        Debug.Log("object " + other.gameObject.name + " has hit the floor");

        if (obj)
        {
            dirtStopwatches.Add(new DirtStopwatch(obj, Time.deltaTime));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            foreach (DirtStopwatch dirtStopwatch in dirtStopwatches)
            {
                if (dirtStopwatch.objectToDirty.gameObject == other.gameObject)
                {
                    dirtStopwatches.Remove(dirtStopwatch);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i=0; i < dirtStopwatches.Count; i++)
        {
            dirtStopwatches[i].objectToDirty.ReduceObjectCondition(
                amountToDirtyPerSecond * dirtStopwatches[i].GetTimePassed()
                );
            Debug.Log("object " + dirtStopwatches[i].objectToDirty.name + "'s quality is now " + dirtStopwatches[i].objectToDirty.m_condition);
        }
    }
}
