using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System;

public class CharaAnimEventCtrl : MonoBehaviour
{
    public delegate void AtkEfCallBack(AtkEffectType type);
    AtkEfCallBack _effectCallBack;
    public void SetEffectEvent(AtkEfCallBack callBack)
    {
        _effectCallBack = callBack;
    }
    public void EffectEvent(AtkEffectType type)
    {
        _effectCallBack?.Invoke(type);
    }
}
