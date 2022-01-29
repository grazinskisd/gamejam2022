using UnityEngine;
public class ShootBehaviour : MonoBehaviour
{
    public Bullet bulletPrototype;
    public float shootSpeed;
    public KeyCode fireKey;
    public Vector3 projectileVelocity;
    public Transform shootOrigin;

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
                var bullet = Instantiate(bulletPrototype, _bulletHolder);
                bullet.transform.position = shootOrigin.position;
                bullet.velocity = projectileVelocity;
            }
        }
    }
}
