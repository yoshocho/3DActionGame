using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitEvent
{
    void SetUp(GameObject owner);
    void HitEvent(Collider col);
}