using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : IAttackEffect
{
    [Header("ƒJƒ‚ç‚ğ—h‚ç‚·•ûŒü")]
    public Vector3 CameraShakeVec;
    public void SetEffect()
    {
        CameraManager.ShakeCam(CameraShakeVec);
    }

    public void SetUp(GameObject owner)
    {

    }
}