using UnityEngine;
using System;

public class SceneSetUp : MonoBehaviour
{
    [SerializeField]
    GameManager.GameState _gameState;
    
    Action _onSceneStart;
    public static SceneSetUp Instance { get; private set; } = null;
    
    private void Awake()
    {
        Instance = this;
        GameManager.Instance.SetUp();
        GameManager.Instance.GameStateEvent(_gameState);
    }

    public void SetInit(Action setUp)
    {
        _onSceneStart += setUp;
    }
    private void Start()
    {
        FadeSystem.FadeIn(() => _onSceneStart?.Invoke());
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
