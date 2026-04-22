using UnityEngine;

public class Jar : MonoBehaviour
{
    public Spread availSpread;
    public Jar()
    {
        availSpread = Spread.PEANUT_BUTTER;
    }

    private void OnTriggerEnter(Collider other)
    {
        Silverware silverwareTarget = other.gameObject.GetComponent <Silverware>();
        if (!silverwareTarget) return;

        // apply jar spread to silverware
        if (silverwareTarget)
        {
            if (silverwareTarget.currentSpread == Spread.NO_SPREAD)
            {
                silverwareTarget.AddSpread(this.availSpread, this.transform);
            }
        }
    }
}
