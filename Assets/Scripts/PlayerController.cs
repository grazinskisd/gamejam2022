using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public LayerMask enemyProjectileMask;

    private Rigidbody2D _ridigbody;

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _ridigbody.MovePosition(_ridigbody.position + (input * Time.fixedDeltaTime * moveSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsInLayerMask(enemyProjectileMask))
        {
            Debug.Log("Player hit a bullet");
        }
    }
}
