using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : MonoBehaviour
{
    public struct AnimStack
    {
        public string stateName;
        public float duration;
    }

    public delegate void CallBack(int param);

    [SerializeField] protected Animator m_anim;
    [SerializeField] bool m_isActiveStart;

    int m_targetLayer = 0;
    float m_duration = 0.0f;

    CallBack m_eventCallBack;
    CallBack m_pbkCallBack;

    Queue<AnimStack> m_animQueue = new Queue<AnimStack>();
    private void Awake()
    {
        m_anim = GetComponentInChildren<Animator>();
        if (!m_isActiveStart)
        {
            DisActive();
        }
    }

    public virtual void Active()
    {
        m_anim.enabled = true;
    }

    public virtual void DisActive()
    {
        m_anim.enabled = false;
    }

    public void ForceUpdate()
    {
        m_anim.Update(0.0f);
    }

    public void SetNormalizedTime(float time, int layer = 0)
    {
        m_anim.Play(0,layer,time);
        m_anim.Update(0.0f);
    }

    public virtual void Play(string stateName, float dur = 0.1f)
    {
        Active();
        m_anim.CrossFadeInFixedTime(stateName,dur);
        m_duration = dur;
    }

    public void PlayQueue(string name, float dur = 0.0f)
    {
        m_animQueue.Enqueue(new AnimStack(){stateName = name,duration = dur});
    }

    public bool IsPlayingAnimatin(int layer = 0)
    {
        if (m_duration > 0.0)  return true;
        var state = m_anim.GetCurrentAnimatorStateInfo(layer);
        if (state.loop) return true;
        return state.normalizedTime < 1.0f;
    }

    public void SetEventDelegate(CallBack cb)
    {
        m_eventCallBack = cb;
    }

    public void SetPlayBackDelegate(CallBack cb ,int target = 0)
    {
        m_pbkCallBack = cb;
        m_targetLayer = target;
    }

    public void AnimationEvent(int evtType)
    {
        m_eventCallBack?.Invoke(evtType);
    }

    protected virtual void Update()
    {
        if (m_anim == null) return;

        if (m_duration > 0.0f)
        {
            m_duration -= Time.deltaTime;
        }
        if (m_pbkCallBack != null)
        {
            if (!IsPlayingAnimatin(m_targetLayer))
            {
                CallBack back = m_pbkCallBack;
                m_pbkCallBack = null;
                back(m_targetLayer);
            }
        }
        if (m_animQueue.Count > 0)
        {
            if (!IsPlayingAnimatin(0))
            {
                var anim = m_animQueue.Dequeue();
                Debug.Log(anim.ToString()); ;
                Play(anim.stateName, anim.duration);
            }
        }
    }
}
