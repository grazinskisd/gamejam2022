﻿using UnityEngine;

public abstract class BaseShootBehaviour : MonoBehaviour
{
    public Color projectileColor;
    public Transform shootOrigin;
    public string bulletLayerName;

    public ShootingBehaviourSettings[] settings;

    private ShootingBehaviourSettings[] _settingsCopy;
    private Transform _bulletHolder;
    private int _bulletLayer;
    private AudioSource _audioSource;

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
        _audioSource = GameObject.Find("SFXAudioSource").GetComponent<AudioSource>();
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
            var didFire = settings.Fire(shootOrigin.position, Angle, _bulletHolder, _bulletLayer, projectileColor);
            if(didFire && settings.shootSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(settings.shootSound);
            }
        }
    }
}
