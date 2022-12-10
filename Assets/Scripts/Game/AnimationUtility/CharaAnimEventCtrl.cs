using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System;

/// <summary>
/// アニメーションイベントで設定する用のBool型
/// </summary>
public enum AnimBool
{
    True,
    False,
}

public class CharaAnimEventCtrl : MonoBehaviour
{
    public delegate void AtkEfCallBack(AtkEffectType type);
    AtkEfCallBack _effectCallBack;
    public delegate void TriggerActiveEvent(AnimBool enable);
    TriggerActiveEvent _triggerEvent;

    Action _atkAction;
    public void SetEffectEvent(AtkEfCallBack callBack)
    {
        _effectCallBack = callBack;
    }
    public void EffectEvent(AtkEffectType type)
    {
        _effectCallBack?.Invoke(type);
    }
    public void SetTriggerEvent(TriggerActiveEvent activeEvent)
    {
        _triggerEvent = activeEvent;
    } 
    public void TriggerEnable(AnimBool enable)
    {
        _triggerEvent?.Invoke(enable);
    }

    public void SetAttackAction(Action atkAction)
    {
        _atkAction = atkAction;
    }
    public void AttackAction()
    {
        _atkAction?.Invoke();
    }
}
