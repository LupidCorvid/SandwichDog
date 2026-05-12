using UnityEngine;
using UnityEngine.Audio;


//This script lowers or increases the volume linearly based on radial position away from the audio source


public class PositionBasedVolume : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] GameObject player;
    public float maxVolume;
    public float range;
    public float innerRange;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Mathf.Abs(player.transform.position.magnitude - transform.position.magnitude);
        audioSource.volume = SetVolume(distance);
    }

    float SetVolume(float distance)
    {
        if (distance > range)
        {
            return 0;
        }
        else if (distance <= innerRange)
        {
            return maxVolume;
        }
        else
        {
            return maxVolume * (innerRange / distance);
        }
    }
}
