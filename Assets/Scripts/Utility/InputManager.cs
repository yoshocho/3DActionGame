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

        ///回避ボタンの入力
        if (m_inputActions.Player.Avoid.WasPressedThisFrame())
            AvoidKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Avoid.IsPressed())
            AvoidKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Avoid.WasReleasedThisFrame())
            AvoidKey = KeyStatus.UP;
        else AvoidKey = KeyStatus.NONE;

        ///ジャンプボタンの入力
        if (m_inputActions.Player.Jump.WasPerformedThisFrame())
            JumpKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Jump.IsPressed())
            JumpKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Jump.WasReleasedThisFrame())
            JumpKey = KeyStatus.UP;
        else JumpKey = KeyStatus.NONE;

        ///攻撃ボタンの入力
        if (m_inputActions.Player.Attack.WasPerformedThisFrame())
            AttackKey = KeyStatus.DOWN;
        else if (m_inputActions.Player.Attack.IsPressed())
            AttackKey = KeyStatus.STAY;
        else if (m_inputActions.Player.Attack.WasReleasedThisFrame())
            AttackKey = KeyStatus.UP;
        else AttackKey = KeyStatus.NONE;


        //if (Input.GetButtonDown("×button") || Input.GetKeyDown(KeyCode.Space))
        //{
        //    m_jumpKey = KeyStatus.DOWN;
        //    Debug.Log("JumpキーDown");
        //}
        //else if (Input.GetButton("×button") || Input.GetKey(KeyCode.Space))
        //{
        //    m_jumpKey = KeyStatus.STAY;
        //    CountUp(ref m_longPushTimer);
        //    if (m_longPushTimer >= m_avoidPushTime)
        //    {
        //        m_jumpKey = KeyStatus.LongPush;
        //        //m_longPushTimer = 0.0f;
        //        Debug.Log("LongPush");
        //    }
        //    Debug.Log("JumpキーStay");
        //}
        //else if (Input.GetButtonUp("×button") || Input.GetKeyUp(KeyCode.Space))
        //{
        //    m_jumpKey = KeyStatus.UP;
        //    m_avoidPushTimer = 0.0f;
        //    Debug.Log("JumpキーUp");
        //}
        //else
        //{
        //    m_jumpKey = KeyStatus.NONE;
        //}

        /////回避キー
        //if (Input.GetButtonDown("L1button") || Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    m_avoidKey = KeyStatus.DOWN;
        //}
        //else if (Input.GetButton("L1button") || Input.GetKey(KeyCode.LeftShift))
        //{
        //    m_avoidKey = KeyStatus.STAY;
        //}
        //else if (Input.GetButtonUp("L1button") || Input.GetKey(KeyCode.LeftShift))
        //{
        //    m_avoidKey = KeyStatus.UP;
        //}
        //else
        //{
        //    m_avoidKey = KeyStatus.NONE;
        //}
        //if (Input.GetButton("L1button") || Input.GetKey(KeyCode.LeftShift))
        //{
        //    m_avoidPushTimer += Time.deltaTime;
        //}
        //else if(Input.GetButtonUp("L1button") || Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    if (m_avoidPushTimer < m_avoidPushTime)
        //    {
        //        Debug.Log("AvoidDown");
        //        m_avoidKey = KeyStatus.DOWN;
        //    }
        //    m_avoidPushTimer = 0.0f;
        //}
        //else
        //{
        //    m_avoidKey = KeyStatus.NONE;
        //}

        //if (m_avoidPushTimer > m_avoidPushTime)
        //{
        //    m_avoidKey = KeyStatus.STAY;
        //    Debug.Log("AvoidSTAY");
        //}
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
