using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCollider<T> : MonoBehaviour
    where T : MonoBehaviour
{
    protected List<T> items = new List<T>();

    protected virtual bool CanAddItem(T script)
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();

        if (!script) return;
        if (!CanAddItem(script)) return;

        if (!items.Any(item => item == script))
        {
            items.Add(script);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        T script = other as T;
        if (!script) return;
        if (!CanAddItem(script)) return; // if it couldn't be added then there's no reason to search for it when removing

        items.Remove(script);
    }
}