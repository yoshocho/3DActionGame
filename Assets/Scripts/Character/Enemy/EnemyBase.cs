using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))] //OnWillRenderObject‚ðŽg‚¤‚½‚ß
public class EnemyBase : CharacterBase
{
    protected Transform _targetTrans;
    protected float _distance;
    public bool IsVisible { get; private set; }
    protected override void SetUp()
    {
        base.SetUp();
        GameManager.Instance.FieldData.RegisterEnemy(this);
        _targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public virtual void Death()
    {
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