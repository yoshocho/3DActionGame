using AttackSetting;
using UnityEngine;
using System;

[RequireComponent(typeof(GroundChecker), typeof(ActionCtrl))]
public partial class NormalStateEnemy : EnemyBase
{
    enum StateType
    {
        Idle,
        Chase,
        Attack,
        Damage,
    }
    enum Attack
    {
        Single,
        Combo,
    }
    [SerializeField, Range(0.5f, 5.0f)]
    float _attackRange = 1.5f;
    [SerializeField, Range(2.0f, 20f)]
    float _waitTime = 2.0f;
    [SerializeField]
    float _gravityScale = 0.98f;
    [SerializeField]
    float _rotateSpeed;


    Transform _selfTrans;
    
    Quaternion _targetRot;
    Vector3 _currentVelocity;

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
        StateCash();
    }
    void ComponentSetUp()
    {
        if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();
        _actCtrl = GetComponent<ActionCtrl>();
        _actCtrl.SetUp(gameObject);
        _mover = GetComponent<RigidMover>();
        _mover.SetUp();
        _mover.SetMoveSpeed = MoveSpeed;
    }

    private void StateCash()
    {
        _stateMachine = new StateMachine<NormalStateEnemy>(this);

        _stateMachine
            .AddAnyTransition<NormalEnemyIdleState>((int)StateType.Idle)
            .AddAnyTransition<NormalEnemyMoveState>((int)StateType.Chase)
            .AddAnyTransition<NormalEnemyAttackState>((int)StateType.Attack)
            .AddAnyTransition<NormalEnemyDamageState>((int)StateType.Damage)
            .Start<NormalEnemyIdleState>();
    }

    protected override void OnUpdate()
    {
        if (IsDeath) return;
        base.OnUpdate();
        _stateMachine.Update();
        _mover.Velocity = new Vector3(_currentVelocity.x,_mover.Velocity.y,_currentVelocity.z);
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
            ServiceLocator<NewFieldManager>.Instance.DeathRequest(this);
            _animCtrl.Play("Death", 0.2f,onAnimEnd:() => Death());
        }
    }
}
