using UnityEngine;
public class ShootBehaviour : MonoBehaviour
{
    public Rigidbody2D bulletPrototype;
    public float shootSpeed;
    public KeyCode fireKey;

    private float _lastShootTime;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetKey(fireKey))
        {
            if (Time.time - _lastShootTime >= (1 / shootSpeed))
            {
            }
        }
    }
}
