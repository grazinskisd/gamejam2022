using UnityEngine;

public abstract class BaseShootBehaviour : MonoBehaviour
{
    private const float AudioTimeTolerance = 0.1f;

    public Transform shootOrigin;
    public string bulletLayerName;

    public ShootingBehaviourSettings[] settings;

    private ShootingBehaviourSettings[] _settingsCopy;
    private Transform _bulletHolder;
    private int _bulletLayer;
    private AudioPlayer _audioPlayer;
    private float _lastTimePlayed;

    protected abstract float Angle { get; }

    private void Awake()
    {
        _settingsCopy = new ShootingBehaviourSettings[settings.Length];
        for (int i = 0; i < settings.Length; i++)
        {
            _settingsCopy[i] = Instantiate(settings[i]);
        }

        _bulletHolder = GameObject.Find("BulletHolder").transform;
        _bulletLayer = LayerMask.NameToLayer(bulletLayerName);
        _audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Update()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        FireFromAllSettings();
    }

    protected void FireFromAllSettings()
    {
        for (int i = 0; i < _settingsCopy.Length; i++)
        {
            var settings = _settingsCopy[i];
            var didFire = settings.Fire(shootOrigin.position, Angle, _bulletHolder, _bulletLayer);
            if (didFire && Time.time - _lastTimePlayed >= AudioTimeTolerance && settings.shootSound != null && _audioPlayer != null)
            {
                _lastTimePlayed = Time.time;
                _audioPlayer.PlayOneShot(settings.shootSound);
            }
        }
    }
}
