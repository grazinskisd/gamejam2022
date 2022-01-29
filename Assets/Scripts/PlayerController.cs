using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private const string RuneTag = "Rune";
    public float moveSpeed = 1;
    public LayerMask enemyProjectileMask;

    public UnityEvent OnMoveStart;

    private Rigidbody2D _ridigbody;
    private bool _hasMovementStarted;
    private Pickup _currentRune;
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
            _currentRune = collision.gameObject.GetComponent<Pickup>();
            _currentRune.gameObject.SetActive(false);
            _currentRune.IssuePickupEvent();
        }

        if (collision.gameObject.CompareTag("RuneDeposit"))
        {
            if (_currentRune != null)
            {
                var deposit = collision.gameObject;
                deposit.GetComponent<DepositSpot>().AddRune(_currentRune);
                _currentRune = null;
            }
        }
    }
}
