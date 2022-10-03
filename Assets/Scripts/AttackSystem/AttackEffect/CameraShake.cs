using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Name("カメラ")]
public class CameraShake : IAttackEffect
{
    [Header("X方向の揺れの強さ"),Range(0,0.3f)]
    public float XShakeVec;
    [Header("Y方向の揺れの強さ"), Range(0, 0.3f)]
    public float YShakeVec;
    [Header("Z方向の揺れの強さ"), Range(0, 0.3f)]
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