using UnityEngine;
public class ShootBehaviour : MonoBehaviour
{
    public float shootSpeed;
    public KeyCode fireKey;
    public Transform shootOrigin;

    public ShootingBehaviourSettings settings;

    private float _lastShootTime;
    private Transform _bulletHolder;

    private void Awake()
    {
        _bulletHolder = new GameObject("Bullet holder").transform;
    }

    private void Update()
    {
        if (Input.GetKey(fireKey))
        {
            if (Time.time - _lastShootTime >= (1 / shootSpeed))
            {
                _lastShootTime = Time.time;
                settings.Fire(shootOrigin.position, -90, _bulletHolder);
            }
        }
    }
}
