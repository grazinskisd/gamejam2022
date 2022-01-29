using UnityEngine;

public abstract class BaseShootBehaviour : MonoBehaviour
{
    public Transform shootOrigin;
    public string bulletLayerName;

    public ShootingBehaviourSettings[] settings;

    private ShootingBehaviourSettings[] _settingsCopy;
    private Transform _bulletHolder;
    private int _bulletLayer;

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
            _settingsCopy[i].Fire(shootOrigin.position, Angle, _bulletHolder, _bulletLayer);
        }
    }
}
