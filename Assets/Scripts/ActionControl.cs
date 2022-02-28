using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int Damage = 100;
    //public AnimationClip TargetAnim;
}
public class ActionControl : MonoBehaviour
{
    [SerializeField,Tooltip("大剣の通常攻撃List")]
    List<Attack> m_heavySwordNormalCombo = new List<Attack>();
    public List<Attack> HeavySwordNormalCombos => m_heavySwordNormalCombo;
    [SerializeField, Tooltip("大剣の空中攻撃List")]
    List<Attack> m_heavySwordAirialCombo = new List<Attack>();
    public List<Attack> HeavySwordAirialCombos => m_heavySwordAirialCombo;
    [SerializeField, Tooltip("大剣のスキル攻撃List")]
    List<Attack> m_heavySwordSkillList = new List<Attack>();
    public List<Attack> HeavySwordSkillList => m_heavySwordSkillList;



    float m_stopTime = default;
    float m_frameTimer = default;
    float m_timeScale = default;
    bool m_isHitStop = default;
    
    HitCtrl m_hitCtrl;
    private void Awake()
    {
        //m_hitCtrl = GetComponentInChildren<HitCtrl>();
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

    public void HitStop(float power)
    {
        m_stopTime = power * 1.0f / 24.0f;

        m_isHitStop = true;
    }
}
