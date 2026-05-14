using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[SerializeField]
public struct Timer<T> where T : ObjClass
{
    private T scriptToAffect;
    private float startTime;

    public Timer(T inObject)
    {
        startTime = Time.deltaTime;
        scriptToAffect = inObject;
    }

    public T GetObject()
    {
        return scriptToAffect;
    }

    public float GetTimePassed()
    {
        return Time.deltaTime - startTime;
    }
}

public class TimerCollider<T> : MonoBehaviour 
    where T : ObjClass
{
    protected List<Timer<T>> timers = new List<Timer<T>>();

    /// <summary>
    /// Inherited classes determine what their timers do here
    /// </summary>
    protected virtual void TickTimer(T script, float timePassed)
    {

    }

    protected virtual bool CanAddTimer(T script)
    {
        return true;
    }

    protected virtual bool ShouldRemoveTimer(T script)
    {
        return false;
    }

    protected virtual void OnTimerRemoved(T script)
    {

    }

    public void AddTimer(T script)
    {
        timers.Add(new Timer<T>(script));
        script.OnRemoveTimers += this.RemoveObjectTimer;
        script.OnReceiveTimers += this.AddObjectTimer;
    }

    public void RemoveTimer(Timer<T> timer)
    {
        OnTimerRemoved(timer.GetObject());
        timer.GetObject().OnReceiveTimers -= this.RemoveObjectTimer;
        timer.GetObject().OnReceiveTimers -= this.AddObjectTimer;
        timers.Remove(timer);
        //if (temp) Debug.Log(timer.GetObject().name + "'s timer on " + this.name + " was removed!");
    }

    private void Update()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            TickTimer(timers[i].GetObject(), Time.deltaTime);

            if (ShouldRemoveTimer(timers[i].GetObject()))
            {
                RemoveTimer(timers[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();
        //Debug.Log(this.name + " received " + other.name);

        // try to get script from parent GO if child failed
        if (!script && other.attachedRigidbody)
        {
            script = other.attachedRigidbody.GetComponent<T>();
        }

        if (!script) return;
        //Debug.Log(this.name + " received right type of " + script.name);
        if (!CanAddTimer(script)) return;

        //Debug.Log(this.name + " will add try to add timer for " + script.name);
        AddObjectTimer(script);
    }

    private void AddObjectTimer(ObjClass obj)
    {
        T templatedObj = obj as T;
        if (templatedObj)
        {
            AddObjectTimer(templatedObj);
        }
    }

    private void AddObjectTimer(T script)
    {
        // get top-most parent
        T parent = script;
        T validParent = null;
        while (parent.objOwner)
        {
            parent = parent.objOwner as T;
            if (parent)
            {
                validParent = parent;
            }
        }

        // we can only add the timer to the parent if it also intersects this collider
        if (validParent != null)
        {
            script = validParent;
            bool stillIntersects = false;
            Collider thisCollider = this.GetComponent<Collider>();
            foreach (Collider collider in validParent.XRGI.colliders)
            {
                if (collider.bounds.Intersects(thisCollider.bounds))
                {
                    stillIntersects = true;
                    break;
                }
            }

            if (!stillIntersects) return;
        }

        // only add a timer if there's not already one present for the incoming obj
        if (!timers.Any(timer => ReferenceEquals(timer.GetObject(), script)))
        {
            Debug.Log(this.name + " will add timer for " + script.name);
            AddTimer(script);
        }
    }

    private void RemoveObjectTimer(ObjClass obj)
    {
        T templatedObj = obj as T;
        if (templatedObj)
        {
            RemoveObjectTimer(templatedObj);
        }
    }

    private void RemoveObjectTimer(T script)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            //Debug.Log(other.name + " timer check");
            if (ReferenceEquals(timers[i].GetObject(), script))
            {
                //Debug.Log(other.name + " remove timer!");
                RemoveTimer(timers[i]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();

        // try to get script from parent GO if child failed
        if (!script && other.attachedRigidbody)
        {
            script = other.attachedRigidbody.GetComponent<T>();
        }

        //Debug.Log(other.name + " has exited the trigger!");

        if (!script) return;

        //Debug.Log(other.name + " is of our templated type");      

        RemoveObjectTimer(script);
    }

    //private void OnTriggerStay(Collider other)
    //{
        //T script = other.gameObject.GetComponentInChildren<T>();

        //if (!script) return;

        //Debug.Log(script.newChildren.Count);

        //while (script.newChildren.Count > 0)
        //{
            //this.AddTimer(script.newChildren.Pop() as T);
        //}
    //}
}
