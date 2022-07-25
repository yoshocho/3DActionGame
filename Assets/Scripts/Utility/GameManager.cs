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
        Loading,
    }

    private static GameManager s_instance = new GameManager();
    private GameManager() { }
    public static GameManager Instance => s_instance ??= s_instance = new GameManager();
    
    ///<summary>現在のゲームステート</summary>
    public GameState CurrentState { get; private set; }
    /// <summary> ゲームの経過時間</summary>
    public TimeData GameTime = new TimeData();
    
    Subject<bool> _onPause = new Subject<bool>();
    /// <summary>ゲームポーズイベント </summary>
    public IObservable<bool> OnPause => _onPause;

    public bool GameStart { get; private set; } = true;

    public FieldData FieldData { get; private set; } = new FieldData();

    public GameObject LockOnTarget { get; set; } = null;

    public void SetUp(GameState state)
    {
        UiManager.Instance.SetUp();

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
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.GameOver:
                
                
                break;
            case GameState.Loading:

                break;
            default:
                break;
        }
    }
    void ResetGameTime()
    {
        GameTime.ResetTime();
    }

    public void UpdateGameTime()
    {
        GameTime.UpdateTime();
    }
}
