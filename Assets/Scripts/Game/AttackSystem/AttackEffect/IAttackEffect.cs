using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackEffect
{
    void SetUp(Transform ownerTrans);
    void SetEffect();
}