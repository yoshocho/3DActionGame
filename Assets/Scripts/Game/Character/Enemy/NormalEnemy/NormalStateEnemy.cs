using AttackSetting;
using UnityEngine;
using System;
using UniRx;

[RequireComponent(typeof(GroundChecker), typeof(ActionCtrl))]
public partial class NormalStateEnemy : EnemyBase
{
    enum StateType
    {
        Idle,
        Move,
        Attack,
        Death,
        Damage,
    }
    enum Attack
    {
        Single,
        Combo,
    }
   
    enum MoveType 
    {
        Walk,
        Stafe,
        Run,
        Stop,
    }

    [SerializeField]
    float _gravityScale = 0.98f;
    [SerializeField]
    float _rotateSpeed;
    [SerializeField]
    float _runSpeed = 3.0f;

    bool _canMove = true;

    MoveType _moveType;
    bool _attackCoolTime = false;
    //float _attackTimer;
    Transform _selfTrans;
    Quaternion _targetRot;
    Attack _attackType = Attack.Single;
    StateMachine<NormalStateEnemy> _stateMachine;
    ActionCtrl _actCtrl;
    AnimationCtrl _animCtrl;
    RigidMover _mover;

    [SerializeField]
    bool _debagMode;

    protected override void SetUp()
    {
        base.SetUp();
        _selfTrans = transform;
        ComponentSetUp();
        GameManager.Instance.OnGameEnd.Subscribe(_ => _canMove = false);
        StateCash();
    }

    void ComponentSetUp()
    {
        if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();
        _actCtrl = GetComponent<ActionCtrl>();
        _actCtrl.SetUp(gameObject);
        _mover = GetComponent<RigidMover>();
        _mover.SetUp(_selfTrans);
        _mover.SetMoveSpeed = _runSpeed;
    }

    private void StateCash()
    {
        _stateMachine = new StateMachine<NormalStateEnemy>(this);

        _stateMachine
            .AddAnyTransition<NormalEnemyIdleState>((int)StateType.Idle)
            .AddAnyTransition<NormalEnemyMoveState>((int)StateType.Move)
            .AddAnyTransition<NormalEnemyAttackState>((int)StateType.Attack)
            .AddAnyTransition<NormalEnemyDamageState>((int)StateType.Damage)
            .AddAnyTransition<NormalEnemyDeathState>((int)StateType.Death)
            .Start<NormalEnemyIdleState>();
    }

    protected override void OnUpdate()
    {
        if (!_canMove) 
        {
            _mover.Velocity = Vector3.zero;
            return;
        }

        base.OnUpdate();
        _stateMachine.Update();
        _mover.SetRot = _targetRot;
    }
     
    void PlayAnim(string stateName,float dur = 0.1f,int layer = 0,Action onAnimEnd = null)
    {
        _animCtrl.Play(stateName, dur,layer,onAnimEnd);
    }
    void ChangeState(StateType state)
    {
        _stateMachine.Dispatch((int)state);
    }

    public override void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        if (IsDeath) return;

        base.AddDamage(damage, attackType);

        if (_debagMode) Debug.Log(Status.CurrentHp);
        if (IsDeath)
        {
            ServiceLocator<FieldManager>.Instance.DeathRequest(this);
            ChangeState(StateType.Death);
        }
    }
}
