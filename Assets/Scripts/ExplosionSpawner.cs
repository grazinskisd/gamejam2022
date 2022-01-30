using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    public GameObject explosionPrototype;
    public AudioClip[] explosionSounds;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void SpawnExplosion(Vector3 position)
    {
        var explosion = Instantiate(explosionPrototype, transform);
        explosion.transform.position = position;
        PlayRandomExplosionSFX();
    }

    private void PlayRandomExplosionSFX()
    {
        _audioPlayer.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
    }
}
