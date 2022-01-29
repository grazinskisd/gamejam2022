using UnityEngine;

[CreateAssetMenu(menuName = "GameJam2022/CircularShooting")]
public class CircularShooting : ShootingBehaviourSettings
{
    public int angleStep = 10;

    protected override void CreateBullet(Vector3 originPosition, float direction, Transform parent, int layer)
    {
        for (int i = 0; i < 360; i += angleStep)
        {
            var adjustedDirection = direction + i;

            var bullet = Instantiate(bulletPrototype, parent);
            bullet.transform.position = originPosition;

            var rotation = Quaternion.AngleAxis(adjustedDirection, Vector3.forward);
            bullet.velocity = rotation * new Vector3(0, projectileSpeed, 0);
            bullet.gameObject.layer = layer;
        }
    }
}
