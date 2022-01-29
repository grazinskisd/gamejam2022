﻿using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float stageMoveSpeed;
    public float stageMoveTime;
    public float bossRetreatTime;

    public GameObject stagesHolder;
    public GameStage[] stages;
    public GameObject introScreen;
    public ParalaxController paralaxController;

    [Header("Player")]
    public PlayerController playerController;
    public ShootBehaviour playerShootBehaviour;
    public DepositSpot depositSpot;

    private int _stageIndex;
    private float _timeMoved;

    private GameState _state = GameState.Init;
    private Vector3 _stageStartPosition;

    private GameStage CurrentStage => stages[_stageIndex];

    private void Awake()
    {
        _stageStartPosition = stagesHolder.transform.position;
        introScreen.SetActive(true);
        playerController.OnMoveStart.AddListener(StartGame);
        playerShootBehaviour.OnShootStart.AddListener(StartGame);

        for (int i = 0; i < stages.Length; i++)
        {
            var stage = stages[i];
            stage.pickupSpot.OnPickup.AddListener(HandleObjectPickup);
            stage.gameObject.SetActive(false);
        }

        CurrentStage.gameObject.SetActive(false);
        paralaxController.SpeedMultiplier = 0;

        depositSpot.OnDeposit.AddListener(EnterNextStage);
    }

    private void EnterNextStage(Pickup arg0)
    {
        if (_state == GameState.WaitingForDeposit || _state == GameState.ReturningBack)
        {
            _state = GameState.GoingToPickup;
            CurrentStage.gameObject.SetActive(false);
            if (_stageIndex + 1 < stages.Length)
            {
                stagesHolder.transform.position = _stageStartPosition;
                _stageIndex++;
                CurrentStage.gameObject.SetActive(true);
                _timeMoved = 0;
            }
            else
            {
                _state = GameState.Finished;
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
            CurrentStage.boss.transform.SetParent(transform);
            _state = GameState.BossChase;
            _timeMoved = 0;
            paralaxController.SpeedMultiplier = 1;
        }
    }

    private void StartGame()
    {
        introScreen.SetActive(false);
        CurrentStage.gameObject.SetActive(true);
        _state = GameState.GoingToPickup;
        paralaxController.SpeedMultiplier = -1;
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
                    _state = GameState.WaitingForPickup;
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
                    _state = GameState.ReturningBack;
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
                    _state = GameState.WaitingForDeposit;
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

    public enum GameState
    {
        Init, GoingToPickup, WaitingForPickup, BossChase, ReturningBack, WaitingForDeposit, Finished
    }
}