using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class EnemyBase : CharacterBase,IPoolObject
{
    protected Transform _targetTrans;

    protected override void SetUp()
    {
        base.SetUp();
        _targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void IPoolObject.SetUp()
    {
        RB.WakeUp();
    }
    void IPoolObject.Sleep()
    {
        RB.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
