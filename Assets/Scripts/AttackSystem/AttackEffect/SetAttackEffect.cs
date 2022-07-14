using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAttackEffect : IAttackEffect
{
    Transform OwnerPos;
    public void SetUp(GameObject owner)
    {
        OwnerPos = owner.transform;
    }
    public void SetEffect()
    {

    }
}
