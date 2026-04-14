using UnityEngine;

public class SinkCollider : MonoBehaviour
{
    public bool canClean;

    private void OnTriggerEnter(Collider other)
    {
        ObjClass otherObject = other.gameObject.GetComponent<ObjClass>();
        if (!otherObject) return;

        if (otherObject.CanGetClean)
        {
            otherObject.RemoveSpreads();
        }
    }
}
