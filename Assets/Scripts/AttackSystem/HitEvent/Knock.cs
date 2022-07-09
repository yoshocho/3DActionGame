using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
