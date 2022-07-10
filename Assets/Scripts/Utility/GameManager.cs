using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

/// <summary>
/// ゲームを管理するクラス
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
    public static GameManager Instance => s_instance ??= s_instance = new GameManager();
    
    ///<summary>現在のゲームステート</summary>
    public GameState CurrentState { get; private set; }
    /// <summary> ゲームの経過時間</summary>
    public float ElapsedTime { get; private set; } = 0.0f;
    
    Subject<bool> _onPause = new Subject<bool>();
    /// <summary>ゲームポーズイベント </summary>
    public IObservable<bool> OnPause => _onPause;

    public bool GameStart { get; private set; } = true;

    List<EnemyBase> _fieldEnemys = new List<EnemyBase>();

    public GameObject LockOnTarget { get; set; }

    public void Register(EnemyBase enemy)
    {
        _fieldEnemys.Add(enemy);
    }

    public void Remove(EnemyBase enemy)
    {
        _fieldEnemys.Remove(enemy);
    }

    public void SetUpEvent(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                //UI
                break;
            case GameState.InGame:
                InputManager.Instance.SetUp();
                break;
            case GameState.GameOver:
                break;
            case GameState.GameEnd:

                //UI
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
                Debug.Log("InGame");
                break;
            case GameState.GameOver:
                //var player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
                
                break;
            case GameState.GameEnd:
                Debug.Log("End");
                break;
            case GameState.Loading:

                break;
            default:
                break;
        }
    }
    void ResetGameTime()
    {
        ElapsedTime = 0.0f;
    }

    public void UpdateGameTime()
    {
        ElapsedTime += Time.deltaTime;
    }
}
