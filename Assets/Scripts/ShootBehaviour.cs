using UnityEngine;
using UnityEngine.Events;

public class ShootBehaviour : BaseShootBehaviour
{
    public UnityEvent OnShootStart;
    public KeyCode fireKey;

    public bool CanFire { get; set; } = true;

    protected override float Angle => -90;
    private bool _wasFirstShotFired;

    protected override void Shoot()
    {
        if (CanFire && Input.GetKey(fireKey))
        {
            if (!_wasFirstShotFired)
            {
                _wasFirstShotFired = true;
                OnShootStart?.Invoke();
            }
            base.Shoot();
        }
    }
}
