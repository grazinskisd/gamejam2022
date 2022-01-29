using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public LayerMask enemyProjectileMask;

    public UnityEvent OnMoveStart;

    private Rigidbody2D _ridigbody;
    private bool _hasMovementStarted;

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(!_hasMovementStarted && input.sqrMagnitude > 0)
        {
            OnMoveStart?.Invoke();
            _hasMovementStarted = true;
        }
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
