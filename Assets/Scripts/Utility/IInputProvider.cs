using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum InputType
{
    None,
    Down,
    Pressed,
    Up,
}
public interface IInputProvider
{
    bool GetJump(InputType type);
    bool GetAttack(InputType type);
    bool GetAvoid(InputType type);
    Vector3 GetInputDirection();
    Vector2 GetCameraAxis();

    IInputProvider SetCallBack(InputAction input,Action onCall);
}
