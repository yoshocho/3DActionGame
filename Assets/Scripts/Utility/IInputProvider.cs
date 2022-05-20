using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider
{
    bool GetJump();
    bool GetAttack();
    bool GetAvoid();
    Vector3 GetMoveDirection();
}
