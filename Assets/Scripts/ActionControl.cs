using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public enum AttackLayer
{
    InFight,
    LongRange,
    Airial
}
public enum ActionType
{
    Animation,
    CreateObject
}
[System.Serializable]
public class Attack
{
    public string Name = "";
    public int Step = 0;
    public AttackLayer Layer = AttackLayer.InFight;
    public ActionType ActType = ActionType.Animation;
    public string ActionTargetName = "";
    public float KeepTime = 1f;
    public float Power = 0.7f; 
    public float WaitTime = 0.5f;
    //public AnimationClip TargetAnim;
}
public class ActionControl : MonoBehaviour
{

    /// <summary>コンボを繋げた時のイベント</summary>
    private Subject<Unit> comboSubject = new Subject<Unit>();
    public IObservable<Unit> OnCombo => comboSubject;

    [SerializeField]
    List<Attack> m_attacts = new List<Attack>();
    public List<Attack> Attacks => m_attacts;
    [SerializeField]
    List<Attack> m_airialAttack = new List<Attack>();
    public List<Attack> AirialAttacks => m_airialAttack;
    [SerializeField]
    List<Attack> m_skillAttacks = new List<Attack>();
    public List<Attack> SkillAttacks => m_skillAttacks;
    //[SerializeField] AnimationCtrl m_animCtrl = default;
    float m_stopTime = default;
    float m_frameTimer = default;
    float m_timeScale = default;
    bool m_isHitStop = default;

    
    HitCtrl m_hitCtrl;
    private void Awake()
    {
        m_hitCtrl = GetComponentInChildren<HitCtrl>();
        m_timeScale = Time.timeScale;
    }


    private void Update()
    {
        if (m_isHitStop && Time.timeScale == m_timeScale)
        {
            m_frameTimer = 0;
            m_timeScale = Time.timeScale;
            Time.timeScale = 0;
            m_isHitStop = false;
        }
        if (Time.timeScale == 0 && m_frameTimer < m_stopTime)
        {
            m_frameTimer += Time.unscaledDeltaTime;
            if (m_frameTimer >= m_stopTime)
            {
                Time.timeScale = m_timeScale;
            }
        }
    }

    public void HitCallBack(IDamage enemys, int actId)
    {
        enemys?.AddDamage(100);
        HitStop(m_attacts[actId].Power);
        comboSubject.OnNext(Unit.Default);
    }

    void HitStop(float power)
    {
        m_stopTime = power * 1.0f / 24.0f;

        m_isHitStop = true;
    }
}
