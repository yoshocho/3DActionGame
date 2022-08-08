using System.Collections.Generic;
using UnityEngine;
using System;
using AttackSetting;

[RequireComponent(typeof(ActionCtrl), typeof(GroundChecker))]
public partial class PlayerStateMachine : CharacterBase
{
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
        Common, �@
        Strafe,  //����������Ă�����
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

    StyleState _currentStyle = StyleState.Common;
    AttackType _attackType;
    Transform _selfTrans;
    Vector3 _moveForward = Vector3.zero;
    Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputAxis = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;

    IInputProvider _inputProvider;
    InputManager _inputManager;
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
    bool _keepAir = false;

    protected override void SetUp()
    {
        base.SetUp();
        Init();
        StateCash();
    }
    void Init()
    {
        InputManager.Instance.PlayerInput.Player.LockOn.started += context => LockOn();

        _selfTrans = transform;
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
        if(!IsGround()) ApplyGravity();
        ApplyMove();
    }

    public void SetInputProvider(IInputProvider input)
    {
        _inputProvider = input;
    }

    void ApplyAxis()
    {
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
        _currentVelocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
    }

    bool IsGround()
    {
        return _grandCheck.IsGround();
    }

    public void LockOn()
    {

        if (GameManager.Instance.LockOnTarget != null)
        {

            GameManager.Instance.LockOnTarget = null;
            UiManager.Instance.ReceiveData("gameUi", new LockOnEventHandler(false));
            Debug.Log("LockOn����");
        }
        else
        {
            Debug.Log("LockOn!");
            var targetObj = CameraManager.Instance.FindTarget(transform, 60.0f, false, true);
            var target = targetObj.GetComponent<CharacterBase>().CenterPos;
            GameManager.Instance.LockOnTarget = target;
            UiManager.Instance.ReceiveData("gameUi", new LockOnEventHandler(true, target.transform));
        }
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
        UiManager.Instance.ReceiveData("gameUi", hpData);

        if (IsDeath)
        {
            GameManager.Instance.GameStateEvent(GameManager.GameState.GameOver);
        }
    }
}