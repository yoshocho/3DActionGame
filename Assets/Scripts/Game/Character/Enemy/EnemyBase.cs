using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionParam
{
    [SerializeField, Range(0.5f, 20.0f)]
    float _attackCoolTime;
    [SerializeField, Range(0, 10.0f)]
    float _attackRange;
    [SerializeField]
    float _battleArea;
    public float AttackRange => _attackRange;
    public float AttackCoolTime => _attackCoolTime;
    public float BattleArea => _battleArea;
}
[RequireComponent(typeof(MeshRenderer))] //OnWillRenderObject‚ðŽg‚¤‚½‚ß
public class EnemyBase : CharacterBase , ITargetable
{
    [SerializeField]
    private ActionParam _actionParam = new ActionParam();
    public ActionParam ActionParam => _actionParam;
    protected Transform _targetTrans;
    protected float _distance;
    public bool IsVisible { get; private set; } = false;

    public bool Targetable => !IsDeath;

    public Transform TargetTransform => Data.CenterPos;

    [SerializeField]
    int _score = 100;

    protected override void SetUp()
    {
        base.SetUp();
        GameManager.Instance.FieldData.RegisterEnemy(this);
        _targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public virtual void Death()
    {
        GameManager.Instance.AddScore(_score);
        Destroy(gameObject);
    }

    private void OnWillRenderObject()
    {
        IsVisible = true;
        //Debug.Log("‰æ–Ê“à‚Å‚·");
    }

    private void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate()
    {
        IsVisible = false;
        _distance = Vector3.Distance(transform.position, _targetTrans.position);
    }
}