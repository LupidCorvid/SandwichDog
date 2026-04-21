using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializeField]
public struct Timer<T> where T : MonoBehaviour
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
    where T : MonoBehaviour
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

    private void Update()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            TickTimer(timers[i].GetObject(), Time.deltaTime);

            if (ShouldRemoveTimer(timers[i].GetObject()))
            {
                timers.Remove(timers[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();

        if (!script) return;
        if (!CanAddTimer(script)) return;

        if (!timers.Any(timer => timer.GetObject() == script))
        {
            timers.Add(new Timer<T>(script));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();

        Debug.Log(other.name + " has exited the trigger!");

        if (!script) return;

        Debug.Log(other.name + " is of our templated type");

        for (int i = 0; i < timers.Count ;i++)
        {
            Debug.Log(other.name + " timer check");
            if (ReferenceEquals(timers[i].GetObject(), script))
            {
                Debug.Log(other.name + " remove timer!");
                timers.Remove(timers[i]);
            }
        }            
    }
}
