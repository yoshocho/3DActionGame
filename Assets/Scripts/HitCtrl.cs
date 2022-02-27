using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class HitCtrl : MonoBehaviour
{
    public Collider m_atkTrigeer;
    PlayerStateMachine m_player;
    //int m_actId;
    Attack m_attack;
    Subject<GameObject> m_hitTarget = new Subject<GameObject>();
    public IObservable<GameObject> HitTarget => m_hitTarget;
    private void Awake()
    {
        m_atkTrigeer = GetComponentInChildren<Collider>();
        m_atkTrigeer.enabled = false;
    }

    private void Start()
    {
        
    }

    public void SetCtrl(PlayerStateMachine player,Attack attack)
    {
        m_player = player;
        m_attack = attack;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (!other.gameObject.CompareTag("Enemy")) return;

        m_hitTarget.OnNext(other.gameObject);

        EffectManager.PlayEffect("Hit",other.ClosestPoint(transform.position));
        var enemy = other.gameObject.GetComponent<EnemyBase>();
        //var enemy = other.gameObject.GetComponent<IDamage>();
        //m_actCtrl.HitCallBack(enemy,m_attack);
        if(enemy is not null) m_player.HitCallBack(enemy, m_attack);
    }
}
