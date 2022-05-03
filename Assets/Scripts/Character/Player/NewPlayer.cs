using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AttackSetting;

[RequireComponent(typeof(ActionCtrl))]
public partial class NewPlayer : CharacterBase
{
    enum StateEvent :int
    {
        Idle,
        Walk,
        Run,
        Avoid,
        Attack,
        Jump,
        Fall,
        Land,
        Damage,
    }

    [SerializeField]
    float _gravityScale = 10;
    [SerializeField]
    float _walkSpeed = 10;
    [SerializeField]
    float _runSpeed = 15;
    [SerializeField]
    float _rotateSpeed = 10;
    [Tooltip("接地判定での中心からの距離")]
    [SerializeField] float m_isGroundLength = 1.05f;
    [Tooltip("接地判定の範囲")]
    [SerializeField] float m_isGroundRadius = 0.18f;
    [Tooltip("地面のレイヤー")]
    [SerializeField] LayerMask m_groundLayer;

    Transform _selfTrans;
    Vector3 _moveForward = Vector3.zero;
    Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputAxis = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;

    float _moveSpeed;
    [SerializeField]
    AnimationCtrl _animCtrl;
    ActionCtrl _actionCtrl;
    StateMachine<NewPlayer> _stateMachine;
    bool _isAvoid = false;
    bool _invincible = false;
    void Start()
    {
        _stateMachine = new StateMachine<NewPlayer>(this);

        _selfTrans = transform;
        if(!_animCtrl)_animCtrl = GetComponentInChildren<AnimationCtrl>();
        _actionCtrl = GetComponent<ActionCtrl>();
    }
    void Update()
    {
        ApplyAxis();
        ApplyMove();
        ApplyRotation();
        ApplyGravity();

        _stateMachine.Update();
    }
    void ApplyAxis()
    {

    }
    void ApplyMove()
    {

    }
    void ApplyRotation()
    {

    }
    void ApplyGravity()
    {

    }
    void ChangeState(StateEvent nextState)
    {
        _stateMachine.Dispatch((int)nextState);
    }
    void PlayAnimation(string name, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    {
        _animCtrl.Play(name,dur,layer,onAnimEnd);
    }
    public override void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        if (_invincible) return;

        if (_isAvoid) 
        {

            return;
        }

        switch (attackType)
        {
            case AttackType.Weak:
                break;
            case AttackType.Heavy:

                break;
            default:
                break;
        }
        base.AddDamage(damage, attackType);
    }

}
