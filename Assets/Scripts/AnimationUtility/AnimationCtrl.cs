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
    [Range(-0.5f, 2.0f)]
    public float Speed = 1.0f;
}

[System.Serializable]
public class AnimState
{
    public string StateName = "";
    [Range(0, 0.5f), Header("ブレンド時間")]
    public float Duration = 0.1f;
}
public class AnimationCtrl : MonoBehaviour
{
    public delegate void CallBack(int param);
    CallBack m_eventCallBack;

    [SerializeField]
    Animator m_anim;

    AnimatorOverrideController m_animatorOverrideController;

    private void Awake()
    {
        if (!m_anim) m_anim = GetComponentInChildren<Animator>();

        m_animatorOverrideController = new AnimatorOverrideController();
        m_animatorOverrideController.runtimeAnimatorController = m_anim.runtimeAnimatorController;
        m_anim.runtimeAnimatorController = m_animatorOverrideController;
    }

    public void Active()
    {
        m_anim.enabled = true;
    }

    public void DisActive()
    {
        m_anim.enabled = false;
    }

    public void ForceUpdate()
    {
        m_anim.Update(0.0f);
    }

    public void SetNormalizedTime(float time, int layer = 0)
    {
        m_anim.Play(0, layer, time);
        m_anim.Update(0.0f);
    }

    //public void Play(string stateName,float dur = 0.1f,int layer = 0)
    //{
    //Active();
    //m_anim.CrossFadeInFixedTime(stateName, dur, layer);
    //}
    public void Play(string stateName, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    {
        Active();
        m_anim.CrossFadeInFixedTime(stateName, dur, layer);
        StartCoroutine(AnimEndCallBack(0, onAnimEnd));
    }

    //public void PlayAndCallBack(string stateName, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    //{

    //}

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
    public void ChangeClip(string stateName, AnimationClip clip, int layerId = 0)
    {

        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[m_anim.layerCount];
        for (int i = 0; i < m_anim.layerCount; i++)
        {
            layerInfo[i] = m_anim.GetCurrentAnimatorStateInfo(i);
        }
        m_animatorOverrideController[stateName] = clip;
        m_anim.Update(0.0f);
        for (int i = 0; i < m_anim.layerCount; i++)
        {
            m_anim.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        }
    }

    public bool IsPlayingAnimatin(int layer = 0)
    {
        var state = m_anim.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true;
        return state.normalizedTime < 1.0f;
    }

    public void SetEventDelegate(CallBack cb)
    {
        m_eventCallBack = cb;
    }

    public void AnimationEvent(int evtType)
    {
        m_eventCallBack?.Invoke(evtType);
    }

}
