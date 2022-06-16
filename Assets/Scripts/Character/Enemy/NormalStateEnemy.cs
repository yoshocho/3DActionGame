using AttackSetting;
using UnityEngine;
using State = StateMachine<NormalStateEnemy>.State;
using ObjectPool;

[RequireComponent(typeof(GrandChecker))]
public partial class NormalStateEnemy : CharacterBase,IPoolObject
{
    enum StateType
    {
        Idle,
        Chase,
        Attack
    }
    enum Attack
    {
        Single,
        Combo,
    }
    [SerializeField,Range(0.5f,5.0f)]
    float _attackRange = 1.5f;
    [SerializeField, Range(0.5f,5.0f)]
    float _waitTime = 2.0f;
    [SerializeField]
    float _rotateSpeed = 5.0f;

    NewHitCtrl _hitCtrl;

    [SerializeField]
    Transform _targetTrans;

    Quaternion _targetRot;
    StateMachine<NormalStateEnemy> _stateMachine;
    ActionCtrl _actCtrl;
    Transform _selfTrans;
    Vector3 _currentVelocity;

    private void Start()
    {
        _stateMachine = new StateMachine<NormalStateEnemy>(this);

        _stateMachine.AddAnyTransition<IdleState>((int)StateType.Idle);
        _stateMachine.AddAnyTransition<ChaseState>((int)StateType.Chase);
        _stateMachine.AddAnyTransition<AttackState>((int)StateType.Attack);

        _selfTrans = transform;
        _targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        _actCtrl = GetComponent<ActionCtrl>();
        if (!_hitCtrl) _hitCtrl = GetComponentInChildren<NewHitCtrl>();
    }
    private void Update()
    {
        ApplyAxis();
        _stateMachine.Update();
    }
    private void FixedUpdate()
    {       
        ApplyMove();
        ApplyRotate();
    }
    void ApplyAxis()
    {
        _currentVelocity = (_currentVelocity - _selfTrans.position).normalized;
    }
    void ApplyMove()
    {
        var velo = Vector3.Scale(_currentVelocity, new Vector3(MoveSpeed, 1.0f, MoveSpeed));
        Rigidbody.velocity = velo;
    }
    void ApplyRotate()
    {
        var rot = _selfTrans.rotation;
        rot = Quaternion.Lerp(rot,_targetRot,_rotateSpeed * Time.deltaTime);
        _selfTrans.rotation = rot;
    }

    public override void AddDamage(int damage, AttackType attackType = AttackType.Weak)
    {
        base.AddDamage(damage, attackType);

        if (IsDeath)
        {

        }
    }

    public void SetUp()
    {
        throw new System.NotImplementedException();
    }

    public void Sleep()
    {
        throw new System.NotImplementedException();
    }

    class ChaseState : State
    {
        protected override void OnEnter(State prevState)
        {
            
        }
        protected override void OnUpdate()
        {
            owner._targetRot = Quaternion.LookRotation(owner._currentVelocity);
        }
        protected override void OnExit(State nextState)
        {
            
        }
    }
    class IdleState : State
    {
        protected override void OnEnter(State prevState)
        {
            owner._currentVelocity.x = 0.0f;
            owner._currentVelocity.y = 0.0f;
        }
        protected override void OnUpdate()
        {

        }
        protected override void OnExit(State nextState)
        {

        }
    }
    class AttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            base.OnExit(nextState);
        }
    }
    class DamageState : State
    {
        protected override void OnEnter(State prevState)
        {
            base.OnEnter(prevState);
        }
    }
}
