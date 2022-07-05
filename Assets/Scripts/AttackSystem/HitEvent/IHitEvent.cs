using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    void SetUp(GameObject owner);
    void HitEvent(Collider col);
}

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
public class Knock : IHitEvent
{
    [SerializeField]
    Vector3 PowerVec;
    public void SetUp(GameObject owner)
    {
        
    }
    public void HitEvent(Collider col)
    {
        Debug.Log("Knock  " + PowerVec);
    }
}
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
        if (EffectPrefab) EffectManager.PlayEffect(EffectPrefab,col.ClosestPoint(OwnerPos.position));
    }
}
