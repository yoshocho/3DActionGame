using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : IHitEvent
{
    [SerializeField, Range(0f, 5.0f)]
    [Header("ヒットストップの値")]
    float HitStopPower = 0.0f;
    public void SetUp(GameObject owner)
    {

    }
    public void HitEvent(Collider col)
    {
        EffectManager.Instance.HitStop(HitStopPower);
    }
}