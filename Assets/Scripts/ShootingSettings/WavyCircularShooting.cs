using UnityEngine;

[CreateAssetMenu(menuName = "GameJam2022/WavyCircularShooting")]
public class WavyCircularShooting : ShootingBehaviourSettings
{
    public float amplitude = 1;
    public float frequency = 1;

    protected override void CreateBullet(Vector3 originPosition, float direction, Transform parent, int layer)
    {
        var adjustedDirection = direction + Mathf.Sin(Time.time * frequency) * amplitude;

        var bullet = Instantiate(bulletPrototype, parent);
        bullet.transform.position = originPosition;

        var rotation = Quaternion.AngleAxis(adjustedDirection, Vector3.forward);
        bullet.velocity = rotation * new Vector3(0, projectileSpeed, 0);
        bullet.gameObject.layer = layer;
    }
}