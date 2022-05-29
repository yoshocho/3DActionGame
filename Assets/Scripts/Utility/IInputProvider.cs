using UnityEngine;

public interface IInputProvider
{
    bool GetJump();
    bool GetAttack();
    bool GetAvoid();
    Vector3 GetInputDirection();
}
