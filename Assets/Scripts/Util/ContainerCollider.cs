using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCollider<T> : MonoBehaviour
    where T : ObjClass
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
        if (!script.enabled) return;
        if (!CanAddItem(script)) return;

        if (!items.Any(item => ReferenceEquals(item, script))) ;
        {
            //Debug.Log(this.name + " will add " + script.name + " into its list");
            items.Add(script);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        T script = other.gameObject.GetComponentInChildren<T>();
        if (!script) return;
        if (!script.enabled) return;

        items.Remove(script);
    }
}