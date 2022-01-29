using UnityEngine;
public class ShootBehaviour : BaseShootBehaviour
{
    public KeyCode fireKey;

    protected override float Angle => -90;

    protected override void Shoot()
    {
        if (Input.GetKey(fireKey))
        {
            base.Shoot();
        }
    }
}
