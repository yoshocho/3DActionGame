using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    void SetUp(Transform ownerTrans);
    void HitEvent(Collider col);
}