using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Name("�J����")]
public class CameraShake : IAttackEffect
{
    [Header("X�����̗h��̋���"),Range(0,0.3f)]
    public float XShakeVec;
    [Header("Y�����̗h��̋���"), Range(0, 0.3f)]
    public float YShakeVec;
    [Header("Z�����̗h��̋���"), Range(0, 0.3f)]
    public float ZShakeVec;

    public void SetUp(Transform ownerTrans)
    {
        throw new System.NotImplementedException();
    }
    public void SetEffect()
    {
        //CameraManager.Instance.ShakeCam(new Vector3(XShakeVec,YShakeVec,ZShakeVec));
    }
}