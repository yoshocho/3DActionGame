using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackEffect : IAttackEffect
{
    [SerializeField]
    string _effectName;
    [SerializeField]
    Vector3 _offsetPos = Vector3.zero;
    [SerializeField]
    Vector3 _targetRotation = Vector3.zero;
    [SerializeField]
    bool _setParent = false;
    Transform _ownerTrans;
    public void SetUp(Transform ownerTrans)
    {
        _ownerTrans = ownerTrans;
    }
    public void SetEffect()
    {
        GameObject effctObj = EffectManager.PlayEffect(_effectName, _ownerTrans.position + _offsetPos,
            Quaternion.Euler(_targetRotation));
        if (_setParent) effctObj.transform.SetParent(_ownerTrans);
    }
}
