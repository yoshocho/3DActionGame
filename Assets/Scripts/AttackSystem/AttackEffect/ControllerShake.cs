using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShake : IAttackEffect
{
    [SerializeField, Header("�R���g���[���[�U���̏c���l")]
    Vector2 ShakeVec;
    [SerializeField, Header("�R���g���[���[�U���̎�������")]
    public float Duration;

    public void SetUp(Transform ownerTrans)
    {

    }
    public void SetEffect()
    {

    }
}