using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public enum GameState
{
    Title,
    InGame,
    GameOver,
    GameEnd,
    Loading,
}

/// <summary>
/// ƒQ[ƒ€‚Ì—¬‚ê‚ğŠÇ—‚·‚éƒNƒ‰ƒX
/// </summary>
public partial class GameManager
{
    private static GameManager m_instance = new GameManager();
    private GameManager() { }
    public static GameManager Instance => m_instance;

    public GameState CurrentState { get; private set; }



    public void GameStateEvent(GameState state)
    {
        CurrentState = state;

        switch (state)
        {
            case GameState.Title:

                break;
            case GameState.InGame:
                
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
}
