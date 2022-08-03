using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System;

[System.Serializable]
public class AnimClip
{
    [SerializeField, Header("アニメーションクリップ")]
    public AnimationClip Clip;
    [Range(0, 0.5f), Header("ブレンド時間")]
    public float Duration = 0.1f;
    [Range(0.1f, 2.0f),Header("アニメーションのスピード")]
    public float Speed = 1.0f;
    [SerializeField]
    public bool UseRootMotion = false;
}

[System.Serializable]
public class AnimState
{
    [SerializeField]
    public string StateName = string.Empty;
    [Range(0, 0.5f), Header("ブレンド時間")]
    public float Duration = 0.1f;
    [SerializeField]
    public bool UseRootMotion = false;
}
/// <summary>
/// アニメーション再生などを管理するサポートクラス
/// </summary>
public class AnimationCtrl : MonoBehaviour
{
    public delegate void CallBack(int param);
    CallBack _eventCallBack;
    [SerializeField]
    Transform _owerPos;
    public Transform OwerPos => _owerPos;
    [SerializeField]
    Animator _anim;
    public Animator Animator => _anim;
    AnimatorOverrideController _animatorOverrideController;

    private void Awake()
    {
        SetUp();
    }

    public void SetUp()
    {
        if (!_anim) _anim = GetComponentInChildren<Animator>();

        if (!_owerPos) _owerPos = transform.root.transform;
        _animatorOverrideController = new AnimatorOverrideController(_anim.runtimeAnimatorController);
        _anim.runtimeAnimatorController = _animatorOverrideController;
    }

    public void Active()
    {
        _anim.enabled = true;
    }

    public void DisActive()
    {
        _anim.enabled = false;
    }

    public void ForceUpdate()
    {
        _anim.Update(0.0f);
    }

    public void SetNormalizedTime(float time, int layer = 0)
    {
        _anim.Play(0, layer, time);
        _anim.Update(0.0f);
    }


    public void Play(string stateName, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    {
        Active();
        _anim.CrossFadeInFixedTime(stateName, dur, layer);
        StartCoroutine(AnimEndCallBack(0, onAnimEnd));
    }
    //public void PlayAndCallBack(string stateName, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    //{

    //}

    public void SetRootAnim()
    {
        _anim.applyRootMotion = true;
    }
    public void SetNormalAnim()
    {
        _anim.applyRootMotion = false;
    }

    public IEnumerator AnimEndCallBack(int layer = 0, Action onAnimEnd = null)
    {
        yield return null;
        yield return new WaitUntil(() => !IsPlayingAnimatin(layer));
        onAnimEnd?.Invoke();
    }
    /// <summary>
    /// アニメーションステートの中のアニメーションを差し替える関数
    /// ＊差し替えるアニメーションステートとその中に最初から同じ名前のアニメーションClipを差し込むこと
    /// </summary>
    /// <param name="stateName">ステート名</param>
    /// <param name="clip">差し込みたいアニメーションクリップ</param>
    /// <param name="layerId">レイヤーId</param>
    public AnimationCtrl ChangeClip(string stateName, AnimationClip clip, int layerId = 0)
    {

        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[_anim.layerCount];
        for (int i = 0; i < _anim.layerCount; i++)
        {
            layerInfo[i] = _anim.GetCurrentAnimatorStateInfo(i);
        }
        _animatorOverrideController[stateName] = clip;
        _anim.Update(0.0f);
        for (int i = 0; i < _anim.layerCount; i++)
        {
            _anim.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        }
        return this;
    }
    public AnimationCtrl SetParameter(string paramName, object parameter)
    {
        switch (parameter)
        {
            case int i:
                _anim.SetInteger(paramName, i);
                break;
            case float f:
                _anim.SetFloat(paramName, f);
                break;
            case bool b:
                _anim.SetBool(paramName, b);
                break;
            default:
                Debug.LogWarning(string.Format("指定された値は使用できません{0}", parameter));
                break;
        }
        return this;
    }
    public bool IsPlayingAnimatin(int layer = 0)
    {
        var state = _anim.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true;
        return state.normalizedTime < 1.0f;
    }

    public void SetEventDelegate(CallBack cb)
    {
        _eventCallBack = cb;
    }

    public void AnimationEvent(int evtType)
    {
        _eventCallBack?.Invoke(evtType);
    }

}
