using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private const string RuneTag = "Rune";
    private const string StopMoveTag = "MoveStop";
    public float moveSpeed = 1;
    public LayerMask enemyProjectileMask;

    public UnityEvent OnMoveStart;

    private Rigidbody2D _ridigbody;
    private bool _hasMovementStarted;
    private GameObject _currentRune;
    private bool _canMove;

    private void Awake()
    {
        _canMove = true;
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (!_hasMovementStarted && input.sqrMagnitude > 0)
            {
                OnMoveStart?.Invoke();
                _hasMovementStarted = true;
            }
            _ridigbody.MovePosition(_ridigbody.position + (input * Time.fixedDeltaTime * moveSpeed));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.IsInLayerMask(enemyProjectileMask))
        {
            Debug.Log("Player hit a bullet");
        }

        if (collision.gameObject.CompareTag(RuneTag))
        {
            _currentRune = collision.gameObject;
            _currentRune.gameObject.SetActive(false);
            _currentRune.GetComponent<Pickup>().IssuePickupEvent();
        }

        if (collision.gameObject.CompareTag(StopMoveTag))
        {
            _canMove = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(StopMoveTag))
        {
            _canMove = true;
        }
    }
}
