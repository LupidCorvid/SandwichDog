using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FloorCollider : MonoBehaviour
{
    private List<Timer<ObjClass>> dirtTimers = new List<Timer<ObjClass>>();

    private void OnTriggerEnter(Collider other)
    {
        ObjClass obj = other.gameObject.GetComponentInChildren<ObjClass>();
        if (!obj) return;

        // add new timer for new objs
        if (!dirtTimers.Any(timer => timer.GetObject() == obj))
        {
            Debug.Log("object " + other.gameObject.name + " has hit the floor");
            dirtTimers.Add(new Timer<ObjClass>(obj));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ObjClass obj = other.gameObject.GetComponentInChildren<ObjClass>();
        if (!obj) return;

        foreach (Timer<ObjClass> dirtTimer in dirtTimers)
        {
            if (dirtTimer.GetObject().gameObject == other.gameObject)
            {
                dirtTimers.Remove(dirtTimer);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i=0; i < dirtTimers.Count; i++)
        {
            dirtTimers[i].GetObject().ReduceObjectCondition();

            if (dirtTimers[i].GetObject().m_condition <= 0)
            {
                Debug.Log("object " + dirtTimers[i].GetObject().name + "'s quality is now " + dirtTimers[i].GetObject().m_condition);                
                dirtTimers.RemoveAt(i);
            }
        }
    }
}
