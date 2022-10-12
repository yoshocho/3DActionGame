using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] //OnWillRenderObject‚ðŽg‚¤‚½‚ß
public class EnemyBase : CharacterBase ,ITargetable
{
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