using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShake : IAttackEffect
{
    [SerializeField, Header("コントローラー振動の縦横値")]
    Vector2 ShakeVec;
    [SerializeField, Header("コントローラー振動の持続時間")]
    public float Duration;

    public void SetUp(Transform ownerTrans)
    {

    }
    public void SetEffect()
    {

    }
}