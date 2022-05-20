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
    float _walkSpeed = 10;
    [SerializeField]
    float _runSpeed = 15;
    [SerializeField]
    float _rotateSpeed = 10;
    [SerializeField]
    float _gravityScale = 10;
    [SerializeField]
    float _jumpPower = 10;
    [SerializeField]
    int _jumpCount = 1;
    [SerializeField]
    int _airDushCount = 1;
    [SerializeField]
    float _avoidTime = 1.0f;
    [SerializeField]
    float _justTime = 0.3f;
    

    [SerializeField]
    bool _invincible = false;

    [SerializeField]
    List<AnimState> _animSets = new List<AnimState>();

    AttackType _currentType;
    Transform _selfTrans;
    Vector3 _moveForward = Vector3.zero;
    Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputAxis = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;

    IInputProvider _inputProvider;
    InputManager _inputManager;
    [SerializeField]
    AnimationCtrl _animCtrl;
    GrandChecker _grandCheck;
    ActionCtrl _actionCtrl;
    StateMachine<NewPlayer> _stateMachine;

    bool _inAvoid = false;
    bool _keepAir = false;
    float _moveSpeed;

    void Start()
    {
        _stateMachine = new StateMachine<NewPlayer>(this);
        _stateMachine.AddAnyTransition<PlayerIdleState>((int)StateEvent.Idle);
        _stateMachine.AddAnyTransition<PlayerWalkState>((int)StateEvent.Walk);
        _stateMachine.AddAnyTransition<PlayerAttackState>((int)StateEvent.Attack);
        _stateMachine.AddAnyTransition<PlayerAvoidState>((int)StateEvent.Avoid);
        _stateMachine.AddAnyTransition<PlayerRunState>((int)StateEvent.Run);
        _stateMachine.AddAnyTransition<PlayerJumpState>((int)StateEvent.Jump);
        _stateMachine.AddAnyTransition<PlayerFallState>((int)StateEvent.Fall);
        _stateMachine.AddAnyTransition<PlayerLandState>((int)StateEvent.Land);
        _stateMachine.Start<PlayerIdleState>();

        _inputManager = InputManager.Instance;
        _selfTrans = transform;
        if(!_animCtrl)_animCtrl = GetComponentInChildren<AnimationCtrl>();
        _grandCheck = GetComponent<GrandChecker>();
        _actionCtrl = GetComponent<ActionCtrl>();
    }
    void Update()
    {
        ApplyAxis();
        _stateMachine.Update();
    }
    private void FixedUpdate()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMove();
    }

    public void SetInputProvider(IInputProvider input)
    {
        _inputProvider = input;
    }

    void ApplyAxis()
    {
        _moveForward = Camera.main.transform.TransformDirection(_inputAxis);
        _moveForward.y = 0.0f;
        _moveForward.Normalize();
        _inputAxis = InputManager.Instance.InputDir;
    }
    void ApplyMove()
    {
        var velocity = Vector3.Scale(_currentVelocity, new Vector3(_moveSpeed, 1.0f, _moveSpeed));
        Rigidbody.velocity = velocity;
    }
    void ApplyRotation()
    {
        var rot = _selfTrans.rotation;
        rot = Quaternion.Slerp(rot,_targetRot,_rotateSpeed * Time.deltaTime);
        _selfTrans.rotation = rot;
    }
    void ApplyGravity()
    {
        if (!IsGround() && !_keepAir)
        {
            _currentVelocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
        }
    }

    bool IsGround()
    {
        return _grandCheck.IsGround();
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

        if (_inAvoid) 
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