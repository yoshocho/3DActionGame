using UnityEngine;
using InputProviders;

public class InputManager
{
    private static InputManager s_instance = new InputManager();
    private InputManager() { }
    public static InputManager Instance => s_instance;

    PlayerInput _inputActions;
    public PlayerInput PlayerInput => _inputActions;
    public static void SetUp()
    {
        s_instance._inputActions = new PlayerInput();
        s_instance._inputActions.Enable();
        var inputProvider = new InputProvider();
        inputProvider.SetUp(s_instance._inputActions);
        ServiceLocator<IInputProvider>.Register(inputProvider);
    }
    public static void Disable() =>s_instance._inputActions?.Disable();
    public static void Dispose() =>s_instance._inputActions?.Dispose();
}
