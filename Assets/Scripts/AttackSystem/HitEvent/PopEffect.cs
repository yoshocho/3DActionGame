using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEffect : IHitEvent
{
    [SerializeField]
    GameObject EffectPrefab;
    public Transform OwnerPos { get; set; }
    public void SetUp(GameObject owner)
    {
        OwnerPos = owner.transform;
    }
    public void HitEvent(Collider col)
    {
        if (EffectPrefab) EffectManager.PlayEffect(EffectPrefab, col.ClosestPoint(OwnerPos.position));
    }
}
