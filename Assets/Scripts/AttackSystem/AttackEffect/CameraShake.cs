using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : IAttackEffect
{
    [Header("X方向の揺れの強さ"),Range(0,0.3f)]
    public float XShakeVec;
    [Header("Y方向の揺れの強さ"), Range(0, 0.3f)]
    public float YShakeVec;
    [Header("Z方向の揺れの強さ"), Range(0, 0.3f)]
    public float ZShakeVec;
    public void SetEffect()
    {
        CameraManager.ShakeCam(new Vector3(XShakeVec,YShakeVec,ZShakeVec));
    }

    public void SetUp(GameObject owner)
    {

    }
}