using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Name("ƒJƒƒ‰")]
public class CameraShake : IAttackEffect
{
    [Header("X•ûŒü‚Ì—h‚ê‚Ì‹­‚³"),Range(0,0.3f)]
    public float XShakeVec;
    [Header("Y•ûŒü‚Ì—h‚ê‚Ì‹­‚³"), Range(0, 0.3f)]
    public float YShakeVec;
    [Header("Z•ûŒü‚Ì—h‚ê‚Ì‹­‚³"), Range(0, 0.3f)]
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