using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity;
    private Rect _boundingRect;

    private void Awake()
    {
        _boundingRect = new Rect(new Vector2(-10, -6), new Vector2(20, 12));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        if (!_boundingRect.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }
}