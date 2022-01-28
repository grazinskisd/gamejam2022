using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;

    private void Update()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += input * Time.deltaTime * moveSpeed;
    }
}
