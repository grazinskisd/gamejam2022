using UnityEngine;

public class ExplosionSpawner : MonoBehaviour
{
    public GameObject explosionPrototype;
    public AudioClip[] explosionSounds;
    private AudioSource _sfxSource;

    private void Awake()
    {
        _sfxSource = GameObject.Find(Globals.SFXSourceName).GetComponent<AudioSource>();
    }

    public void SpawnExplosion(Vector3 position)
    {
        var explosion = Instantiate(explosionPrototype, transform);
        explosion.transform.position = position;
        PlayRandomExplosionSFX();
    }

    private void PlayRandomExplosionSFX()
    {
        _sfxSource.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
    }
}
