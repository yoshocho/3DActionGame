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
    float m_horizontalKey = default;
    public float HorizontalKey => m_horizontalKey;

    float m_verticalKey = default;
    public float VerticalKey => m_verticalKey;

    Vector3 m_inputDir = default;
    public Vector3 InputDir => m_inputDir;

    KeyStatus m_jumpKey = KeyStatus.NONE;
    public KeyStatus JumpKey => m_jumpKey;

    KeyStatus m_attackKey = KeyStatus.NONE;
    public KeyStatus AttackKey => m_attackKey;

    KeyStatus m_avoidKey = KeyStatus.NONE;
    public KeyStatus AvoidKey => m_avoidKey;

    float m_longPushTime = 0.6f;

    float m_longPushTimer = default;

    [SerializeField]
    float m_avoidPushTime = 0.2f;

    float m_avoidPushTimer = default;

    private void Reset()
    {
        m_avoidPushTime = 0.2f;
    }
    Vector2 velo;
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        velo = callbackContext.ReadValue<Vector2>();
        //velon = velo / velo;
        //velo.x = callbackContext.ReadValue<float>();
    }

    private void Update()
    {
        Debug.Log(velo);
        m_inputDir = Vector3.forward * velo.y + Vector3.right * velo.x;
        //m_inputAxis = new Vector2(h,v);
        //m_inputDir = Vector3.forward * v + Vector3.right * h;

        if (Gamepad.current.leftShoulder.wasPressedThisFrame || Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            m_avoidKey = KeyStatus.DOWN;
        }
        else if(Gamepad.current.leftShoulder.isPressed || Keyboard.current.leftShiftKey.isPressed)
        {
            m_avoidKey = KeyStatus.STAY;
        }
        else if (Gamepad.current.leftShoulder.wasReleasedThisFrame || Keyboard.current.leftShiftKey.wasReleasedThisFrame)
        {
            m_avoidKey = KeyStatus.UP;
        }
        else
        {
            m_avoidKey = KeyStatus.NONE;
        }

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


        //if (Input.GetButtonDown("Circlebutton") || Input.GetMouseButtonDown(0))
        //{
        //    m_attackKey = KeyStatus.DOWN;
        //    Debug.Log("攻撃キーDown");
        //}
        //else if (Input.GetButton("Circlebutton") || Input.GetMouseButton(0))
        //{
        //    m_attackKey = KeyStatus.STAY;
        //    Debug.Log("攻撃キーStay");
        //}
        //else if (Input.GetButtonUp("Circlebutton") || Input.GetMouseButtonUp(0))
        //{
        //    m_attackKey = KeyStatus.UP;
        //    Debug.Log("攻撃キーUp");
        //}
        //else
        //{
        //    m_attackKey = KeyStatus.NONE;
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

}
