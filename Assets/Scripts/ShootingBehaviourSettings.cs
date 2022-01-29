using UnityEngine;

[CreateAssetMenu(menuName = "GameJam2022/ShootingBehaviourSettins")]
public class ShootingBehaviourSettings : ScriptableObject
{
    public Bullet bulletPrototype;
    public float projectileSpeed;

    public Bullet Fire(Vector3 originPosition, float direction, Transform parent)
    {
        var bullet = Instantiate(bulletPrototype, parent);
        bullet.transform.position = originPosition;

        var rotation = Quaternion.AngleAxis(direction, Vector3.forward);
        bullet.velocity = rotation * new Vector3(0, projectileSpeed, 0);

        return bullet;
    }
}
