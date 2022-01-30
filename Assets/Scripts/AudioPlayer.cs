using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public float sfxBufferTime = 0.1f;

    private float _lastTimePlayed;
    private AudioSource _audioSource;

    private void Awake()
    {
        _lastTimePlayed = Time.time;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (Time.time - _lastTimePlayed >= sfxBufferTime)
        {
            _lastTimePlayed = Time.time;
            _audioSource.PlayOneShot(clip);
        }
    }
}
