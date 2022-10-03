using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackEffect : IAttackEffect
{
    [SerializeField]
    string _effectName;
    Transform _ownerTrans;
    public void SetUp(Transform ownerTrans)
    {
        _ownerTrans = ownerTrans;
    }
    public void SetEffect()
    {
        //EffectManager.PlayEffect(_effectName,);
    }
}
