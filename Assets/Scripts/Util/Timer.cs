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

public class TimerHolder<T> : MonoBehaviour 
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

    private void FixedUpdate()
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
        T script = other as T;
        if (!script) return;
        if (!CanAddTimer(script)) return; // if it couldn't be added then there's no reason to search for it when removing

        foreach (Timer<T> timer in timers)
        {
            if (timer.GetObject() == script)
            {
                timers.Remove(timer);
            }
        }            
    }
}
