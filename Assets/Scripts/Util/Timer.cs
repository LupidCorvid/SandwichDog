using UnityEngine;

[SerializeField]
public struct Timer<T> where T : MonoBehaviour
{
    private T objectToAffect;
    private float startTime;

    public Timer(T inObject)
    {
        startTime = Time.deltaTime;
        objectToAffect = inObject;
    }

    public T GetObject()
    {
        return objectToAffect;
    }

    public float GetTimePassed()
    {
        return Time.deltaTime - startTime;
    }
}
