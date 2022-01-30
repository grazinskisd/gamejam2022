using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;
    private ExplosionSpawner _explosionController;

    private void Awake()
    {
        _explosionController = FindObjectOfType<ExplosionSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Bullet>(out var bullet))
        {
            health -= bullet.damage;
        }

        if (health <= 0)
        {
            Debug.Log("Enemy death");
            _explosionController.SpawnExplosion(transform.position);
            Destroy(gameObject);
        }
    }
}
