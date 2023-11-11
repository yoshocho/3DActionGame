using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : IHitEvent
{
    [SerializeField, Range(0f, 5.0f)]
    [Header("ヒットストップの値")]
    float HitStopPower = 0.6f;
    public void SetUp(Transform ownerTrans)
    {

    }
    public void HitEvent(Collider col)
    {
        ServiceLocator<EffectManager>.Instance.HitStop(HitStopPower);
    }
}