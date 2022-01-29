using UnityEngine;

public class IdleMover : MonoBehaviour
{
    public float moveSpeed;

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveSpeed, 0, 0) * Time.fixedDeltaTime;
    }
}