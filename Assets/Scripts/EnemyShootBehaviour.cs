using UnityEngine;

public class EnemyShootBehaviour : BaseShootBehaviour
{
    protected override float Angle => 90;

    protected override void Shoot()
    {
        if (Globals.BoundingRect.Contains(transform.position))
        {
            base.Shoot();
        }
    }
}
