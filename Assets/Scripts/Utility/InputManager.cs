using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using InputProviders;

public class InputManager : SingleMonoBehaviour<InputManager>
{
    PlayerInput _inputActions;

    public void SetUp()
    {
        _inputActions = new PlayerInput();
        _inputActions.Enable();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        var inputProvider = new InputProvider();
        inputProvider.SetUp(_inputActions);
        player.SetInputProvider(inputProvider);
    }
    public void Disable()
    {
        _inputActions?.Disable();

    }
    public void Dispose()
    {
        _inputActions?.Dispose();
    }
    public IEnumerator ControllerShake(Vector2 shakeVec, float shakeTime)
    {
        if (Gamepad.current == null) yield break;

        var gamePad = Gamepad.current;
        gamePad.SetMotorSpeeds(shakeVec.x, shakeVec.y);
        yield return new WaitForSeconds(shakeTime);
        gamePad.SetMotorSpeeds(0.0f, 0.0f);
    }
}
