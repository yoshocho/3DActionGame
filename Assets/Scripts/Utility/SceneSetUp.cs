using UnityEngine;
using UnityEngine.Events;

public class SceneSetUp : MonoBehaviour
{
    [SerializeField]
    GameManager.GameState _gameState;
    [SerializeField]
    UnityEvent _onSceneStart;

    private void Awake()
    {
        GameManager.Instance.GameStateEvent(_gameState);
    }
    private void Start()
    {
        FadeSystem.FadeIn(() => _onSceneStart?.Invoke());
    }
}
