using System.Collections.Generic;
using UnityEngine;
using System;
using AttackSetting;

[RequireComponent(typeof(RigidMover))]
public partial class PlayerStateMachine : CharacterBase
{
    /// <summary>
    /// プレイヤーの行動ステート
    /// </summary>
    public enum StateEvent : int
    {
        Idle,
        Run,
        Sprint,
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
    float _jumpTime = 2.0f;
    [SerializeField]
    int _jumpCount = 1;
    [SerializeField]
    float _airDeceleration = 0.3f;
    [SerializeField]
    int _airDushCount = 1;
    [SerializeField]
    float _avoidTime = 1.0f;
    [SerializeField]
    float _avoidSpeed = 2.0f;
    [SerializeField]
    float _justTime = 0.3f;
    [SerializeField]
    WeaponType _currentWeapon;
    [SerializeField]
    bool _invincible = false;

    [SerializeField]
    List<AnimState> _animSets = new List<AnimState>();


    StyleState _currentStyle = StyleState.Common;
    [SerializeField]
    AttackType _attackType;
    Transform _selfTrans;
    Vector3 _moveForward = Vector3.zero;
    //Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputAxis = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;

    IInputProvider _inputProvider;
    [SerializeField]
    AnimationCtrl _animCtrl;
    RigidMover _mover;
    PlayerActionCtrl _playerActCtrl;
    ActionCtrl _actionCtrl;
    StateMachine<PlayerStateMachine> _stateMachine;

    [SerializeField]
    bool _debagMode;
    int _currentJumpCount = 0;
    bool _inAvoid = false;
    bool _keepAir = true;
    bool _canMove = true;
    bool _useAnimVelo = false;
    protected override void SetUp()
    {
        _inputProvider = ServiceLocator<IInputProvider>.Instance;
        InputManager.Instance.PlayerInput.Player.LockOn.started += context => LockOn();
        InputManager.Instance.PlayerInput.Player.WeaponChange.started += context => ChangeWeapon();
        //InputManager.Instance.PlayerInput.Player.Teleport.started += context => Teleport();
        _selfTrans = transform;
        base.SetUp();
        ComponentSetUp();
        StateCash();
    }

    void ComponentSetUp()
    {
        if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();
        //_animCtrl.SetOnAnimEv(AnimatorMove);
        _mover = GetComponent<RigidMover>();
        _mover.SetUp(_selfTrans);
        _mover.SetMoveSpeed = _walkSpeed;
        _playerActCtrl = GetComponent<PlayerActionCtrl>();
        _playerActCtrl.SetUp();
        _actionCtrl = GetComponent<ActionCtrl>();
        _actionCtrl.SetUp(gameObject);
    }
    private void StateCash()
    {
        _stateMachine = new StateMachine<PlayerStateMachine>(this);
        _stateMachine
            .AddAnyTransition<PlayerIdleState>((int)StateEvent.Idle)
            .AddAnyTransition<PlayerWalkState>((int)StateEvent.Run)
            .AddAnyTransition<PlayerAttackState>((int)StateEvent.Attack)
            .AddAnyTransition<PlayerAvoidState>((int)StateEvent.Avoid)
            .AddAnyTransition<PlayerRunState>((int)StateEvent.Sprint)
            .AddAnyTransition<PlayerJumpState>((int)StateEvent.Jump)
            .AddAnyTransition<PlayerFallState>((int)StateEvent.Fall)
            .AddAnyTransition<PlayerLandState>((int)StateEvent.Land)
            .Start<PlayerIdleState>();
    }

    void Update()
    {
        if (!_canMove)
        {
            _mover.Velocity = new Vector3(0.0f, _mover.Velocity.y, 0.0f);
            return;
        }

        ApplyAxis();
        _stateMachine.Update();
        _mover.SetRot = _targetRot;
    }
    void ApplyAxis()
    {
        if (_inputProvider == null) return;
        _inputAxis = _inputProvider.GetInputDirection();
        _moveForward = Camera.main.transform.TransformDirection(_inputAxis);
        _moveForward.y = 0.0f;
        _moveForward.Normalize();
    }
    bool IsGround()
    {
        return _mover.IsGround();
    }

    void LockOn()
    {
        if (CamManager.Instance.IsLockOn == false)
        {
            CamManager.Instance.LockOn(true, 30, false, true);
            Debug.Log("LockOn");
            return;
        }
        CamManager.Instance.LockOn(false);
        Debug.Log("LockOnEnd");
    }

    private void AnimatorMove()
    {
        //if (_useAnimVelo)
        //{
        //    Vector3 deltapos = _animCtrl.Animator.deltaPosition;
        //    deltapos.y = _mover.Velocity.y;

        //    transform.position += deltapos;
        //}

    }

    void Teleport()
    {
        if (GameManager.Instance.LockOnTarget == null) return;

        Transform targetPos = GameManager.Instance.LockOnTarget;

        Vector3 vec = targetPos.position + -targetPos.forward;
        vec.y = targetPos.position.y;
        _selfTrans.transform.position = vec;
    }

    private void ChangeWeapon()
    {
        if (_currentWeapon == WeaponType.LightSword) _currentWeapon = WeaponType.HeavySword;
        else if (_currentWeapon == WeaponType.HeavySword) _currentWeapon = WeaponType.LightSword;

        _playerActCtrl.SetStyle(_currentWeapon);
    }

    void DoRotate(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        dir.y = 0.0f;
        _targetRot = Quaternion.LookRotation(dir);
    }
    void ChangeState(StateEvent nextState)
    {
        _stateMachine.Dispatch((int)nextState);
    }
    public void PlayAnimation(string name, float dur = 0.1f, int layer = 0, Action onAnimEnd = null)
    {
        if (!_animCtrl) return;
        _animCtrl.Play(name, dur, layer, onAnimEnd);
    }
    public override void AddDamage(int damage)
    {
        if (_invincible) return;

        if (_inAvoid)
        {
            //
            return;
        }

        base.AddDamage(damage);

        PlayerHPEventHandler hpData = new PlayerHPEventHandler
        {
            hp = Status.CurrentHp.Value,
            maxHp = Status.MaxHp
        };
        if (_debagMode) Debug.Log(JsonUtility.ToJson(hpData));

        ServiceLocator<UiManager>.Instance.ReceiveData("gameUi", hpData);

        if (IsDeath)
        {
            GameManager.Instance.GameStateEvent(GameManager.GameState.GameOver);
            _animCtrl.Play("Death", 0.1f);
            _canMove = false;
        }
    }
}
