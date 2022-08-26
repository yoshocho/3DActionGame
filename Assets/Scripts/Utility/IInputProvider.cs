using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum InputType
{
    None,
    Down,
    Up,
    Pressed
}
public interface IInputProvider
{
    bool GetJump();
    bool GetAttack();
    bool GetAvoid();
    bool GetAvoidDown();
    Vector3 GetInputDirection();
    Vector2 GetCameraAxis();

    IInputProvider SetCallBack(InputAction input,Action onCall);
}
