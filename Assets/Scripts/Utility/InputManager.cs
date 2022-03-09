using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyStatus
{
    NONE,
    STAY,
    UP,
    DOWN,
    LongPush,
}

public class InputManager : Singleton<InputManager>
{
    public Vector3 InputDir { get; private set; } = default;
    public KeyStatus JumpKey { get; private set; } = KeyStatus.NONE;
    public KeyStatus AttackKey { get; private set; } = KeyStatus.NONE;
    public KeyStatus AvoidKey { get; private set; } = KeyStatus.NONE;

    public KeyStatus WeaponChangeKey { get; private set; } = KeyStatus.NONE;

    public KeyStatus LunchKey { get; private set; }
    [SerializeField]
    float m_avoidPushTime = 0.2f;

    float m_avoidPushTimer = default;

    PlayerInput m_inputActions;

    private void OnEnable()
    {
        m_inputActions = new PlayerInput();
        m_inputActions.Enable();
    }

    private void Update()
    {
        //Debug.Log(velo);

        ///移動方向のベクトル
        var axis = m_inputActions.Player.Move.ReadValue<Vector2>();
        InputDir = Vector3.forward * axis.y + Vector3.right * axis.x;

        ///回避ボタン
        if (m_inputActions.Player.Avoid.WasPressedThisFrame())
            AvoidKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Avoid.IsPressed())
            AvoidKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Avoid.WasReleasedThisFrame())
            AvoidKey = KeyStatus.UP;
        else AvoidKey = KeyStatus.NONE;

        ///ジャンプボタン
        if (m_inputActions.Player.Jump.WasPerformedThisFrame())
            JumpKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Jump.IsPressed())
            JumpKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Jump.WasReleasedThisFrame())
            JumpKey = KeyStatus.UP;
        else JumpKey = KeyStatus.NONE;

        ///攻撃ボタン
        if (m_inputActions.Player.Attack.WasPerformedThisFrame())
            AttackKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Attack.IsPressed())
            AttackKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Attack.WasReleasedThisFrame())
            AttackKey = KeyStatus.UP;
        else AttackKey = KeyStatus.NONE;

        ///武器変更ボタン
        if (m_inputActions.Player.WeaponChange.WasPerformedThisFrame())
            WeaponChangeKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.WeaponChange.IsPressed())
            WeaponChangeKey = KeyStatus.STAY;
        else if (m_inputActions.Player.WeaponChange.WasReleasedThisFrame())
            WeaponChangeKey = KeyStatus.UP;
        else WeaponChangeKey = KeyStatus.NONE;

        if (m_inputActions.Player.Lunch.WasPerformedThisFrame())
            LunchKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Lunch.IsPressed())
            LunchKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Lunch.WasReleasedThisFrame())
            LunchKey = KeyStatus.UP;
        else LunchKey = KeyStatus.NONE;


    }

    private void OnDisable()
    {
        m_inputActions?.Disable();
    }

    private void OnDestroy()
    {
        m_inputActions.Dispose();
    }
}
