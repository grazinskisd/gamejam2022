using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private const string RuneTag = "Rune";

    public int health = 100;
    public float healthRegenSpeed = 1;
    public float moveSpeed = 1;
    public LayerMask enemyProjectileMask;
    public PlayerHealth healthView;
    public UnityEvent OnMoveStart;
    public UnityEvent OnPlayerDeath;

    private Rigidbody2D _ridigbody;
    private bool _hasMovementStarted;
    private Pickup _currentRune;
    private int _maxHealth;
    private bool _canMove;
    private float _timeSinceHeal;

    private void Awake()
    {
        _maxHealth = health;
        _canMove = true;
        _ridigbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _timeSinceHeal += Time.deltaTime;

        if(_timeSinceHeal >= (1f / healthRegenSpeed))
        {
            _timeSinceHeal = 0;
            health = Mathf.Clamp(health + 1, 0, _maxHealth);
        }

        healthView.SetFillAmount((float)health / _maxHealth);
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
            if (collision.gameObject.TryGetComponent<Bullet>(out var bullet))
            {
                health -= bullet.damage;
                if(health <= 0)
                {
                    OnPlayerDeath?.Invoke();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        if (collision.gameObject.CompareTag(RuneTag))
        {
            _currentRune = collision.gameObject.GetComponent<Pickup>();
            if (_currentRune.CanPickUp)
            {
                _currentRune.gameObject.SetActive(false);
                _currentRune.IssuePickupEvent();
                _currentRune.CanPickUp = false;
            }
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
