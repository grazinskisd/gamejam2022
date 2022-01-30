using UnityEngine;

[CreateAssetMenu(menuName = "GameJam2022/ShootingBehaviourSettins")]
public class ShootingBehaviourSettings : ScriptableObject
{
    public Bullet bulletPrototype;
    public float projectileSpeed;
    public float shootSpeed;
    public AudioClip shootSound;

    private float _lastShootTime;

    public bool Fire(Vector3 originPosition, float direction, Transform parent, int layer)
    {
        if (Time.time - _lastShootTime >= (1 / shootSpeed))
        {
            _lastShootTime = Time.time;
            CreateBullet(originPosition, direction, parent, layer);
            return true;
        }
        return false;
    }

    protected virtual void CreateBullet(Vector3 originPosition, float direction, Transform parent, int layer)
    {
        var bullet = Instantiate(bulletPrototype, parent);
        bullet.transform.position = originPosition;

        var rotation = Quaternion.AngleAxis(direction, Vector3.forward);
        bullet.velocity = rotation * new Vector3(0, projectileSpeed, 0);
        bullet.gameObject.layer = layer;
    }
}
