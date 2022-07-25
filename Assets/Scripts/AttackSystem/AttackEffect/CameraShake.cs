using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : IAttackEffect
{
    [Header("X•ûŒü‚Ì—h‚ê‚Ì‹­‚³"),Range(0,0.3f)]
    public float XShakeVec;
    [Header("Y•ûŒü‚Ì—h‚ê‚Ì‹­‚³"), Range(0, 0.3f)]
    public float YShakeVec;
    [Header("Z•ûŒü‚Ì—h‚ê‚Ì‹­‚³"), Range(0, 0.3f)]
    public float ZShakeVec;
    public void SetEffect()
    {
        CameraManager.ShakeCam(new Vector3(XShakeVec,YShakeVec,ZShakeVec));
    }

    public void SetUp(GameObject owner)
    {

    }
}