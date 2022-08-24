using UnityEngine;
using InputProviders;

public class InputManager
{
    private static InputManager s_instance = new InputManager();
    private InputManager() { }
    public static InputManager Instance => s_instance;

    PlayerInput _inputActions;
    public PlayerInput PlayerInput => _inputActions;
    public void SetUp()
    {
        _inputActions = new PlayerInput();
        _inputActions.Enable();
        
    }

    public void SetProvider()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        var inputProvider = new InputProvider();
        inputProvider.SetUp(_inputActions);
        player.ReceiveInputProvider(inputProvider);
    }

    public void Disable() => _inputActions?.Disable();

    public void Dispose() => _inputActions?.Dispose();
}
