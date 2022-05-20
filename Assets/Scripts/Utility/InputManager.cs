using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyStatus
{
    NONE,
    STAY,
    UP,
    DOWN,
}

public class InputManager : SingleMonoBehaviour<InputManager>
{
    public Vector3 InputDir { get; private set; } = default;
    public KeyStatus JumpKey { get; private set; } = KeyStatus.NONE;
    public KeyStatus AttackKey { get; private set; } = KeyStatus.NONE;
    public KeyStatus AvoidKey { get; private set; } = KeyStatus.NONE;

    public KeyStatus WeaponChangeKey { get; private set; } = KeyStatus.NONE;

    public KeyStatus LunchKey { get; private set; }

    public PlayerInput InputActions { get; private set; }

    private void OnEnable()
    {
        InputActions = new PlayerInput();
        InputActions.Enable();
    }

    public void SetUp()
    {
        InputActions = new PlayerInput();
        InputActions.Enable();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();
        var inputProvider = new InputProvider();
        inputProvider.SetUp(InputActions);
        player.SetInputProvider(inputProvider);
    }
    public void Disable()
    {
        InputActions?.Disable();

    }
    public void Dispose()
    {
        InputActions?.Dispose();

    }

    private void Update()
    {
        
        var axis = InputActions.Player.Move.ReadValue<Vector2>();
        InputDir = Vector3.forward * axis.y + Vector3.right * axis.x;

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
