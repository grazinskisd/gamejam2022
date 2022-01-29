using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public Vector3 velocity;

    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }
}
