using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// ÉQÅ[ÉÄÇä«óùÇ∑ÇÈÉNÉâÉX
/// </summary>
public partial class GameManager
{
    public enum GameState
    {
        Title,
        InGame,
        GameOver,
        GameEnd,
        Loading,
    }

    private static GameManager s_instance = new GameManager();
    private GameManager() { }
    public static GameManager Instance => s_instance;

    public GameState CurrentState { get; private set; }

    public float ElapsedTime { get; private set; } = 0.0f;

    Subject<Unit> _onPause = new Subject<Unit>();
    public IObservable<Unit> OnPause => _onPause;

    Subject<Unit> _PauseEnd = new Subject<Unit>();
    public IObservable<Unit> PauseEnd => _PauseEnd;


    public void SetUpScene(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                break;
            case GameState.InGame:
                InputManager.Instance.SetUp();
                break;
            case GameState.GameOver:
                break;
            case GameState.GameEnd:
                break;
            case GameState.Loading:
                break;
            default:
                break;
        }
    }
    public void GameStateEvent(GameState state)
    {
        CurrentState = state;

        switch (state)
        {
            case GameState.Title:                
                Debug.Log("Title");

                break;
            case GameState.InGame:

                break;
            case GameState.GameOver:
                var player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
                
                break;
            case GameState.GameEnd:
                
                break;
            case GameState.Loading:

                break;
            default:
                break;
        }
    }
    void Initialize() 
    {
        ElapsedTime = 0.0f;
    }
}
