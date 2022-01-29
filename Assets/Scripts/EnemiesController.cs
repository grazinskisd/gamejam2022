using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public Vector3 velocity;
    public float moveTime;

    private float _timeMoved;

    private void FixedUpdate()
    {
        if (_timeMoved < moveTime)
        {
            _timeMoved += Time.fixedDeltaTime;
            transform.position += velocity * Time.fixedDeltaTime;
        }
    }
}
