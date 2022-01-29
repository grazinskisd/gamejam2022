using UnityEngine;
public class ShootBehaviour : MonoBehaviour
{
    public KeyCode fireKey;
    public Transform shootOrigin;
    public string bulletLayerName;

    public ShootingBehaviourSettings settings;

    private ShootingBehaviourSettings _settingsCopy;
    private Transform _bulletHolder;
    private int _bulletLayer;

    private void Awake()
    {
        _settingsCopy = Instantiate(settings);
        _bulletHolder = new GameObject("Bullet holder").transform;
        _bulletLayer = LayerMask.NameToLayer(bulletLayerName);
    }

    private void Update()
    {
        if (Input.GetKey(fireKey))
        {
            _settingsCopy.Fire(shootOrigin.position, -90, _bulletHolder, _bulletLayer);
        }
    }
}
