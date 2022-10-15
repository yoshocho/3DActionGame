using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// ゲームを管理するクラス
/// </summary>
public class GameManager
{
    public enum GameState
    {
        None,
        GameStart,
        InGame,
        GameOver,
        GameClear,
    }

    #region singleton
    private static GameManager s_instance = new GameManager();
    private GameManager() { }
    public static GameManager Instance => s_instance ??= s_instance = new GameManager();
    #endregion

    ///<summary>現在のゲームステート</summary>
    public GameState CurrentState { get; private set; }
    /// <summary> ゲームの経過時間</summary>
    public TimeData GameTime { get; private set; } = new TimeData();

    public ScoreData Score { get; private set; } = new ScoreData();

    Subject<bool> _onPause = new Subject<bool>();
    /// <summary>ゲームポーズイベント </summary>
    public IObservable<bool> OnPause => _onPause;

    Subject<Unit> _onGameEnd = new Subject<Unit>();

    public IObservable<Unit> OnGameEnd => _onGameEnd;

    public bool GameStart { get; private set; } = true;

    public FieldData FieldData { get; private set; } = new FieldData();

    public Transform LockOnTarget { get; set; } = null;

    public void SetUp()
    {
        UiManager.Instance.SetUp();
        ServiceLocator<UiManager>.Register(UiManager.Instance);
        InputManager.SetUp();

    }
    public void GameStateEvent(GameState state)
    {
        CurrentState = state;

        switch (state)
        {
            case GameState.GameStart:

                break;
            case GameState.InGame:
                CursorManager.CursorCtrl(false, CursorLockMode.Locked);
                break;
            case GameState.GameOver:
                _onGameEnd.OnNext(Unit.Default);
                CursorManager.CursorCtrl(true, CursorLockMode.None);
                ServiceLocator<UiManager>.Instance.RequestOpen("gameOver");
                break;
            case GameState.GameClear:
                Debug.Log("GameClear");
                var ui = ServiceLocator<UiManager>.Instance;
                ui.RequestOpen("result");
                ui.ReceiveData("result", new ResultData(Score, GameTime));
                CursorManager.CursorCtrl(true, CursorLockMode.None);
                break;
            default:
                break;
        }
    }


    public void ChangeScene(string name)
    {
        Score = new ScoreData();
        GameTime = new TimeData();
        InputManager.Dispose();
        LockOnTarget = null;
        SceneChanger.Instance.FadeScene(name);
    }

    public void AddScore(int score)
    {
        Score.AddScore(score);
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
