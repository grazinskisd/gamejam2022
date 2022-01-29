using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bullet>(out var bullet))
        {
            health -= bullet.damage;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
