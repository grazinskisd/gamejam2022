using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public Vector3 velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        if (!Globals.BoundingRect.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }
}