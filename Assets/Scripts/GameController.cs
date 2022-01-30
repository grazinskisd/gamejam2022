using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private const string IsMovingAnimatorBool = "IsMoving";

    public float stageMoveSpeed;
    public float stageMoveTime;
    public float bossRetreatTime;

    public GameObject stagesHolder;
    public GameStage[] stages;
    public GameObject introScreen;
    public ParalaxController paralaxController;
    public Animator bossMover;

    [Header("Player")]
    public PlayerController playerController;
    public DepositSpot depositSpot;
    public PickupSpot pickupSpot;
    public ShootBehaviourLevelController playerShootingLevel;

    [Header("Game state events")]
    //GoingToPickup, WaitingForPickup, BossChase, ReturningBack, WaitingForDeposit, Finished
    public UnityEvent OnGameStart;
    public UnityEvent OnGoingToPickup;
    public UnityEvent OnWaitingForPickup;
    public UnityEvent OnBossChase;
    public UnityEvent OnReturningBack;
    public UnityEvent OnWaitingForDeposit;
    public UnityEvent OnFinished;

    private int _stageIndex;
    private float _timeMoved;

    private GameState _state = GameState.Init;
    private Vector3 _stageStartPosition;
    private ShootBehaviour[] _playerShootBehaviours;

    private GameStage CurrentStage => stages[_stageIndex];

    private void Awake()
    {
        _stageStartPosition = stagesHolder.transform.position;
        introScreen.SetActive(true);
        playerController.OnMoveStart.AddListener(StartGame);

        _playerShootBehaviours = playerController.GetComponents<ShootBehaviour>();
        for (int i = 0; i < _playerShootBehaviours.Length; i++)
        {
            _playerShootBehaviours[i].OnShootStart.AddListener(StartGame);
        }

        for (int i = 0; i < stages.Length; i++)
        {
            var stage = stages[i];
            stage.gameObject.SetActive(false);
        }

        pickupSpot.OnPickup.AddListener(HandleObjectPickup);

        CurrentStage.gameObject.SetActive(false);
        paralaxController.SpeedMultiplier = 0;

        depositSpot.OnDeposit.AddListener(EnterNextStage);
    }

    private void SetPlayerShootEnabled(bool isEnabled)
    {
        for (int i = 0; i < _playerShootBehaviours.Length; i++)
        {
            _playerShootBehaviours[i].CanFire = isEnabled;
        }
    }

    private void EnterState(GameState state)
    {
        _state = state;
        switch (state)
        {
            case GameState.GoingToPickup:
                OnGoingToPickup?.Invoke();
                break;
            case GameState.WaitingForPickup:
                OnWaitingForPickup?.Invoke();
                break;
            case GameState.BossChase:
                OnBossChase?.Invoke();
                break;
            case GameState.ReturningBack:
                OnReturningBack?.Invoke();
                break;
            case GameState.WaitingForDeposit:
                OnWaitingForDeposit?.Invoke();
                break;
            case GameState.Finished:
                OnFinished?.Invoke();
                break;
            default:
                break;
        }
    }

    private void EnterNextStage(Pickup arg0)
    {
        if (_state == GameState.WaitingForDeposit || _state == GameState.ReturningBack)
        {
            SetPlayerShootEnabled(true);
            playerController.transform.rotation = Quaternion.Euler(0, 0, 0);
            playerShootingLevel.IncrementLevel();

            EnterState(GameState.GoingToPickup);
            CurrentStage.gameObject.SetActive(false);
            if (_stageIndex + 1 < stages.Length)
            {
                stagesHolder.transform.position = _stageStartPosition;
                _stageIndex++;
                CurrentStage.gameObject.SetActive(true);
                _timeMoved = 0;
                paralaxController.SpeedMultiplier = -1;
            }
            else
            {
                EnterState(GameState.Finished);
            }
        }
    }

    private void HandleObjectPickup(Pickup arg0)
    {
        if (_state == GameState.WaitingForPickup)
        {
            CurrentStage.gameObject.SetActive(false);
            _stageIndex++;
            CurrentStage.gameObject.SetActive(true);
            CurrentStage.boss.transform.SetParent(bossMover.transform);
            bossMover.SetBool(IsMovingAnimatorBool, true);
            EnterState(GameState.BossChase);
            playerController.transform.rotation = Quaternion.Euler(0, 180, 0);
            _timeMoved = 0;
            paralaxController.SpeedMultiplier = 1;
            pickupSpot.gameObject.SetActive(false);
        }
    }

    private void StartGame()
    {
        if (_state == GameState.Init)
        {
            introScreen.SetActive(false);
            CurrentStage.gameObject.SetActive(true);
            EnterState(GameState.GoingToPickup);
            paralaxController.SpeedMultiplier = -1;
            OnGameStart?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case GameState.Init:
                break;
            case GameState.GoingToPickup:
                if (_timeMoved < stageMoveTime)
                {
                    _timeMoved += Time.fixedDeltaTime;
                    stagesHolder.transform.position += new Vector3(-stageMoveSpeed, 0, 0) * Time.fixedDeltaTime;
                }
                else
                {
                    Destroy(CurrentStage.enemies.gameObject);
                    paralaxController.SpeedMultiplier = 0;
                    SetPlayerShootEnabled(false);
                    EnterState(GameState.WaitingForPickup);
                }
                break;
            case GameState.WaitingForPickup:
                break;
            case GameState.BossChase:
                if (_timeMoved < bossRetreatTime)
                {
                    _timeMoved += Time.fixedDeltaTime;
                    stagesHolder.transform.position += new Vector3(stageMoveSpeed, 0, 0) * Time.fixedDeltaTime;
                }
                else
                {
                    CurrentStage.boss.transform.SetParent(CurrentStage.transform);
                    bossMover.SetBool(IsMovingAnimatorBool, false);
                    EnterState(GameState.ReturningBack);
                }
                break;
            case GameState.ReturningBack:

                if (_timeMoved < stageMoveTime)
                {
                    _timeMoved += Time.fixedDeltaTime;
                    stagesHolder.transform.position += new Vector3(stageMoveSpeed, 0, 0) * Time.fixedDeltaTime;
                }
                else
                {
                    pickupSpot.gameObject.SetActive(true);
                    EnterState(GameState.WaitingForDeposit);
                    paralaxController.SpeedMultiplier = 0;
                }
                break;
            case GameState.WaitingForDeposit:
                break;
            case GameState.Finished:
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public enum GameState
    {
        Init, GoingToPickup, WaitingForPickup, BossChase, ReturningBack, WaitingForDeposit, Finished
    }
}
