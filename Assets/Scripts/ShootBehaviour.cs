using UnityEngine;
using UnityEngine.Events;

public class ShootBehaviour : BaseShootBehaviour
{
    public UnityEvent OnShootStart;
    public KeyCode fireKey;

    protected override float Angle => -90;
    private bool _wasFirstShotFired;

    protected override void Shoot()
    {
        if (Input.GetKey(fireKey))
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
