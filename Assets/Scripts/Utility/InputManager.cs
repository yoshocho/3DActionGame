using UnityEngine;
using InputProviders;

public class InputManager
{
    private static InputManager s_instance = new InputManager();
    private InputManager() { }
    public static InputManager Instance => s_instance;

    PlayerInput _inputActions;

    public void SetUp()
    {
        _inputActions = new PlayerInput();
        _inputActions.Enable();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        var inputProvider = new InputProvider();
        inputProvider.SetUp(_inputActions);
        player.SetInputProvider(inputProvider);

        _inputActions.Ui.Escape.started += context => Debug.Log("UiOpen");

    }
    public void Disable() => _inputActions?.Disable();

    public void Dispose()
    {
        _inputActions?.Dispose(); 
    }
}
