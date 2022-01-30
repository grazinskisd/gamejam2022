using UnityEngine;

[CreateAssetMenu(menuName = "GameJam2022/TrippleShooting")]
public class TrippleShooting : ShootingBehaviourSettings
{
    public float angleOffset = 30;

    protected override void CreateBullet(Vector3 originPosition, float direction, Transform parent, int layer)
    {
        for (int i = -1; i <= 1 ; i++)
        {
            var adjustedDirection = direction + i * angleOffset;

            var bullet = Instantiate(bulletPrototype, parent);
            bullet.transform.position = originPosition;

            var rotation = Quaternion.AngleAxis(adjustedDirection, Vector3.forward);
            bullet.velocity = rotation * new Vector3(0, projectileSpeed, 0);
            bullet.gameObject.layer = layer;
        }
    }
}
