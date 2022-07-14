using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class EnemyBase : CharacterBase
{
    protected Transform _targetTrans;

    protected override void SetUp()
    {
        base.SetUp();
        _targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
