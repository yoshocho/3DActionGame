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
        Vector3 pos = _ownerTrans.position +
                _ownerTrans.up * _offsetPos.y +
                _ownerTrans.right * _offsetPos.x +
                _ownerTrans.forward * _offsetPos.z;

        GameObject effctObj = EffectManager.PlayEffect(_effectName, pos,Quaternion.Euler(_targetRotation));
        if (_setParent) effctObj.transform.SetParent(_ownerTrans);
    }
}
