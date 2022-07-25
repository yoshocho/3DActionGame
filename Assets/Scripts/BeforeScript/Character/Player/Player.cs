using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;
using AttackSetting;

[RequireComponent(typeof(CharacterController), typeof(AttackAssistController))]
public partial class Player : MonoBehaviour, IDamage
{
    #region parameter
    [Tooltip("移動スピード")]
    float _moveSpeed = default;
    [Tooltip("ダッシュの速さ")]
    [SerializeField] float _dashSpeed = 10f;
    [Tooltip("歩くスピード")]
    [SerializeField] float _walkSpeed = 6f;
    [Tooltip("playerの振り向きスピード")]
    [SerializeField] float _rotateSpeed = 10f;
    [Tooltip("重力の大きさ")]
    [SerializeField] float _gravityScale = 10f;
    [Tooltip("ジャンプ力")]
    [SerializeField] float _jumpPower = 10f;
    [Tooltip("ジャスト回避の無敵時間")]
    [SerializeField] float _justTime = 0.3f;
    [Tooltip("回避移動のスピード")]
    [SerializeField] float _avoidSpeed = 1.2f;
    [Tooltip("回避移動の時間")]
    [SerializeField] float _avoidEndTime = 0.6f;
    [Tooltip("接地判定での中心からの距離")]
    [SerializeField] float _isGroundLength = 1.05f;
    [Tooltip("接地判定の範囲")]
    [SerializeField] float _isGroundRadius = 0.18f;
    [Tooltip("地面のレイヤー")]
    [SerializeField] LayerMask _groundLayer;
    #endregion

    #region status
    [Tooltip("プレイヤーの体力")]
    [SerializeField]
    private IntReactiveProperty m_hp = new IntReactiveProperty(100);
    [Tooltip("プレイヤーの最大体力")]
    [SerializeField]
    private IntReactiveProperty m_maxHp = new IntReactiveProperty(100);
    public IReadOnlyReactiveProperty<int> OnDamage => m_hp;
    public IReadOnlyReactiveProperty<int> MaxHp => m_maxHp;

    #endregion

    #region Event
    /// <summary>ジャスト回避のイベント</summary>
    private Subject<Unit> justsubject = new Subject<Unit>();
    public IObservable<Unit> OnJust => justsubject;
    /// <summary> プレイヤーが死んだ時のイベント</summary>
    private Subject<Unit> playerDeath = new Subject<Unit>();
    public IObservable<Unit> PlayerDeath => playerDeath;
    /// <summary>攻撃ヒット時のイベント</summary>
    private Subject<Unit> comboSubject = new Subject<Unit>();
    public IObservable<Unit> OnCombo => comboSubject;

    #endregion

    Transform _selfTrans;
    CharacterController _controller;
    Animator _anim;
    AttackAssistController _attackAssist;
    WeaponHolder _weaponHolder;
    [SerializeField] ActionControl _actionCtrl;
    [SerializeField] AnimationCtrl _animCtrl;


    #region State
    PlayerStateBase _currentState;
    IdleState _idleState = new IdleState();
    WalkState _walkState = new WalkState();
    AvoidState _avoidState = new AvoidState();
    AttackState _attackState = new AttackState();
    RunState _runState = new RunState();
    JumpState _jumpState = new JumpState();
    FallState _fallState = new FallState();
    LandState _landState = new LandState();
    DeathState _deathState = new DeathState();
    #endregion

    #region Attack
    bool m_poseKeep = false;

    List<Attack> m_currentAttackList = new List<Attack>();
    List<Attack> m_currentAirialAttackList = new List<Attack>();
    List<Attack> m_currentSkillList = new List<Attack>();
    [SerializeField]
    WeaponType m_weaponType = WeaponType.HEAVY;
    #endregion

    bool _inKeepAir = default;

    [SerializeField]
    float _airDasuSpeed = 1.0f;

    [SerializeField]
    float _airDasuTime = 0.5f;

    [SerializeField]
    float _airKeepTime = 1.0f;

    float _airKeepTimer;

    bool _airAttackEnd = default;

    [SerializeField]
    int _jumpStep = 2;
    int _currentJumpStep = default;

    [SerializeField]
    int _airDushCount = 1;
    int _currentAirDushCount = default;
    bool _stateKeep;

    float _currentRotateSpeed = 10f;
    float _currentJustTime = default;
    float _currentGravityScale = default;
    /// <summary>ジャスト回避のトリガー</summary>
    bool _justTrigger = false;

    IInputProvider _inputProvider;
    Vector3 _moveForward = Vector3.zero;
    Vector3 _currentVelocity = Vector3.zero;
    Vector3 _inputDir = Vector3.zero;
    Quaternion _targetRot = Quaternion.identity;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        _attackAssist = GetComponent<AttackAssistController>();
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
        ChangeState(_idleState);
        _selfTrans = transform;
        _currentJustTime = _justTime;
        _currentGravityScale = _gravityScale;
        _currentJumpStep = _jumpStep;
    }

    void Update()
    {
        ApplyAxis();
        ApplyMove();
        ApplyGravity();
        ApplyRotation();

        _currentState.OnUpdate(this);
    }

    public void SetInputProvider(IInputProvider input)
    {
        _inputProvider = input;
    }

    void ApplyAxis()
    {
        _inputDir = _inputProvider.GetInputDirection();
        _moveForward = Camera.main.transform.TransformDirection(_inputDir);
        _moveForward.y = 0.0f;
        _moveForward.Normalize();
    }

    void ApplyMove()
    {
        var velocity = Vector3.Scale(_currentVelocity, new Vector3(_moveSpeed, 1f, _moveSpeed));
        _controller.Move(Time.deltaTime * velocity);
    }

    void ApplyRotation()
    {
        var rot = _selfTrans.rotation;
        rot = Quaternion.Slerp(rot, _targetRot, _currentRotateSpeed * Time.deltaTime);
        _selfTrans.rotation = rot;
    }

    void LookAt(Vector3 target)
    {
        var targetRot = Quaternion.LookRotation(target);
        var rot = _selfTrans.rotation;
        rot = Quaternion.Slerp(rot, targetRot, 1000.0f);
        _selfTrans.rotation = rot;
    }
    //IEnumerator MoveForwardAsync(float time, float speed)
    //{
    //    while (time < 0.0f)
    //    {
    //        time -= Time.deltaTime;
    //        m_currentVelocity += transform.forward * speed * Time.deltaTime;
    //        yield return null;
    //    }
    //}

    bool IsGround()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        Ray ray = new Ray(start, Vector3.down);
        bool isGround = Physics.SphereCast(ray, _isGroundRadius, _isGroundLength, _groundLayer);
        return isGround;
    }

    private void OnDrawGizmos()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        Vector3 end = start + Vector3.down * _isGroundLength;
        Color color = new Color(1, 0, 0);
        Gizmos.DrawWireSphere(end, _isGroundRadius);
    }

    void ApplyGravity()
    {
        
        if (_inKeepAir)
        {
            _currentVelocity.y = 0.0f;
        }
        else if (!(IsGround() || _controller.isGrounded) && !_inKeepAir)
        {
            _currentVelocity.y += _currentGravityScale * Physics.gravity.y * Time.deltaTime;
        }
    }

    public void PlayAnimation(string stateName, float transitionTime = 0.1f, int layer = 0,Action onAnimEnd = null)
    {
        _animCtrl.Play(stateName, transitionTime, layer, onAnimEnd);
    }

    void ChangeState(PlayerStateBase nextState)
    {
        _currentState?.OnExit(this, nextState);
        nextState?.OnEnter(this, _currentState);
        _currentState = nextState;
    }
    public void AddDamage(int damage,AttackType attackType = default)
    {
        if (_justTrigger)
        {
            justsubject.OnNext(Unit.Default);
        }
        else
        {
            m_hp.Value -= damage;
            if (m_hp.Value <= 0)
            {
                playerDeath.OnNext(Unit.Default);
                _controller.enabled = false;
                ChangeState(_deathState);
            }
        }
    }

    void AttackAssist()
    {
        if (_attackAssist.Target)
        {
            var target = _attackAssist.Target.transform.position;
            target.y = _selfTrans.position.y;
            transform.LookAt(target);
        }
        var dir = _selfTrans.forward;
        dir.y = 0.0f;
        _targetRot = Quaternion.LookRotation(dir, Vector3.up);
    }
}
