using System.Collections.Generic;
using UnityEngine;
using System;
using AttackSetting;

[RequireComponent(typeof(ActionCtrl), typeof(GroundChecker))]
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


    enum StyleState 
    {
        Common, 　
        Strafe,  //武器を持っている状態
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
    float _avoidSpeed = 2.0f;
    [SerializeField]
    float _justTime = 0.3f;
    [SerializeField]
    WeaponType _currentWeapon;
    [SerializeField]
    bool _invincible = false;

    [SerializeField]
    List<AnimState> _animSets = new List<AnimState>();


    StateEvent _currentState;
    StyleState _currentStyle = StyleState.Common;
    AttackType _attackType;
    Transform _selfTrans;
    Vector3 _moveForward = Vector3.zero;
    Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputAxis = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;

    IInputProvider _inputProvider;
    [SerializeField]
    AnimationCtrl _animCtrl;
    GroundChecker _grandCheck;
    PlayerActionCtrl _playerActCtrl;
    ActionCtrl _actionCtrl;
    StateMachine<PlayerStateMachine> _stateMachine;

    [SerializeField]
    bool _debagMode;
    int _currentJumpCount = 0;
    bool _inAvoid = false;
    bool _keepAir = true;

    protected override void SetUp()
    {
        _inputProvider = ServiceLocator<IInputProvider>.Instance;
        InputManager.Instance.PlayerInput.Player.LockOn.started += context => LockOn();
        InputManager.Instance.PlayerInput.Player.WeaponChange.started += context => ChangeWeapon();
        _selfTrans = transform;
        base.SetUp();
        ComponentSetUp();
        StateCash();
    }

    void ComponentSetUp()
    {
        if (!_animCtrl) _animCtrl = GetComponentInChildren<AnimationCtrl>();
        _grandCheck = GetComponent<GroundChecker>();
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
        ApplyAxis();
        _stateMachine.Update();
    }
    private void FixedUpdate()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMove();
    }

    void ApplyAxis()
    {
        if (_inputProvider == null) return;
        _inputAxis = _inputProvider.GetInputDirection();
        _moveForward = Camera.main.transform.TransformDirection(_inputAxis);
        _moveForward.y = 0.0f;
        _moveForward.Normalize();
    }
    void ApplyMove()
    {
        var velocity = Vector3.Scale(_currentVelocity, new Vector3(MoveSpeed, 1.0f, MoveSpeed));
        RB.velocity = velocity;
    }
    void ApplyRotation()
    {
        var rot = _selfTrans.rotation;
        rot = Quaternion.Slerp(rot, _targetRot, _rotateSpeed * Time.deltaTime);
        _selfTrans.rotation = rot;
    }
    void ApplyGravity()
    {
        if (!IsGround())
        {
            Debug.Log("ApplyGravity");
            _currentVelocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
        }
        //else
        //{
        //    _currentVelocity.y = 0.0f;
        //}
    }

    bool IsGround()
    {
        return _grandCheck.IsGround();
    }

    public void LockOn()
    {
        if(CamManager.Instance.IsLockOn == false)
        {
            CamManager.Instance.LockOn(true, 30, false, true);
            Debug.Log("LockOn");
            return;
        }
        CamManager.Instance.LockOn(false);
        Debug.Log("LockOnEnd");
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
        }
    }
}
