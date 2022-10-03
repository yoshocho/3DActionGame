using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Name("エフェクト")]
[System.Serializable]
public class PopEffect : IHitEvent
{
    [SerializeField]
    GameObject EffectPrefab;
    public Transform OwnerTrans { get; set; }
    public void SetUp(Transform ownerTrans)
    {
        OwnerTrans = ownerTrans;
    }
    public void HitEvent(Collider col)
    {
        if (EffectPrefab) EffectManager.PlayEffect(EffectPrefab, col.ClosestPoint(OwnerTrans.position));
    }
}
