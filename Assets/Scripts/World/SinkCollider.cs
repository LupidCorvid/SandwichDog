using UnityEngine;

public class SinkCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ObjClass otherObject = other.gameObject.GetComponent<ObjClass>();
        if (!otherObject) return;

        if (otherObject.CanGetClean)
        {
            otherObject.AddSpread(Spread.NO_SPREAD, this.transform);
            if (otherObject.CanGetDirty) otherObject.CleanObject(1.0f);
        }
    }
}
