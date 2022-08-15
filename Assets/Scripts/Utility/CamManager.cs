using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[ExecuteInEditMode]
public class CamManager : MonoBehaviour
{
    [System.Serializable]
    public class CameraParameter
    {
        public Transform FollowTarget;
        public Vector3 Position;
        public Vector3 Angles = new Vector3(10.0f, 0.0f, 0.0f);
        public float Distance = 7.0f;
        public float FiedOfView = 45.0f;
        public Vector3 OffsetPotion = new Vector3(0.0f, 1.0f, 0.0f);
        public Vector3 OffsetAngles;
    }

    enum CamState
    {
        None,
        Control,
        LockOn,
    }

    public static CamManager Instance { get; private set; } = null;

    [SerializeField]
    Transform _parent;
    [SerializeField]
    Transform _child;
    [SerializeField]
    Camera _cam;
    [SerializeField]
    CameraParameter _parameter;
    public CameraParameter Parameter { get { return _parameter; } set { _parameter = value; } }
    [SerializeField]
    float _dampingValue = 6.0f;
    [SerializeField]
    float _verticalAngleMinLimit = -30.0f;
    [SerializeField]
    float _verticalAngleMaxLimit = 50;

    [SerializeField, Header("êÖïΩä¥ìx")]
    float _horizontalSensitivity = 0.5f;
    [SerializeField, Header("êÇíºä¥ìx")]
    float _verticalSensitivity = 0.5f;
    [SerializeField, Header("êÖïΩëÄçÏãtì]")]
    bool _invartX = false;
    [SerializeField, Header("êÇíºëÄçÏãtì]")]
    bool _invartY = false;


    //LockOn
    public bool IsLockOn { get; private set; }
    Quaternion _targetRot;
    Quaternion _newRotation;
    float _targetVerticalAngle;
    Vector3 _planarDirection;
    ITargetable _target;
    public ITargetable Target { get => _target; }


    [SerializeField]
    CamState _camState;
    float _horizontalAngle = 0.0f;
    float _verticalAngle = 10.0f;
    PlayerInput _input;

    
    private void Start()
    {
        _input = InputManager.Instance.PlayerInput;
        Instance = this;
        _verticalAngle = _parameter.Angles.y;
        _horizontalAngle = _parameter.Angles.x;
    }

    //ÉfÉoÉbÉOóp
    //private void LateUpdate()
    //{
    //    ApplyCam();
    //}

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        switch (_camState)
        {
            case CamState.None:
                break;
            case CamState.Control:
                ControlCam();
                break;
            case CamState.LockOn:
                LockOnCam();
                break;
            default:
                break;
        }

        ApplyCam();
    }

    void LockOnCam()
    {
        Vector3 camToTarget = _target.TargetTransform.position - _cam.transform.position;
        Vector3 planarCamToTarget = Vector3.ProjectOnPlane(camToTarget, Vector3.up);

        _planarDirection = planarCamToTarget != Vector3.zero ? planarCamToTarget.normalized : _planarDirection;

        _targetRot = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);

        _newRotation = Quaternion.Slerp(_cam.transform.rotation, _targetRot, Time.deltaTime * 9.0f);
        _parameter.Angles = _newRotation.eulerAngles;

        _horizontalAngle = _targetRot.eulerAngles.x;
        _verticalAngle = _targetRot.eulerAngles.y;
    }

    void ApplyCam()
    {
        if (_child == null || _parent == null || _cam == null) return;
        _parent.position = _parameter.Position;
        _parent.eulerAngles = _parameter.Angles;

        var childPos = _child.localPosition;
        childPos.z = _parameter.Distance;
        _child.localPosition = childPos;

        _cam.fieldOfView = _parameter.FiedOfView;
        _cam.transform.localPosition = _parameter.OffsetPotion;
        _cam.transform.localEulerAngles = _parameter.OffsetAngles;

        if (_parameter.FollowTarget != null)
        {
            _parameter.Position = Vector3.Lerp(_parameter.Position,
                _parameter.FollowTarget.position, Time.deltaTime * _dampingValue);
        }
        _parent.position = _parameter.Position;
    }

    void ControlCam()
    {
        //Debug.Log(axis.x + ":" + axis.y);
        Vector2 inputAxis = _input.Player.CameraAxis.ReadValue<Vector2>();

        _horizontalAngle += _invartX ? -inputAxis.x * _horizontalSensitivity : inputAxis.x * _horizontalSensitivity;
        _verticalAngle -= _invartY ? -inputAxis.y * _verticalSensitivity : inputAxis.y * _verticalSensitivity;


        _verticalAngle = ClampAngle(_verticalAngle, _verticalAngleMinLimit, _verticalAngleMaxLimit);

        _newRotation = Quaternion.Euler(new Vector3(_verticalAngle, _horizontalAngle));
        _parameter.Angles = _newRotation.eulerAngles;
        
    }


    public ITargetable FindTarget(float dis, bool disCenter = false, bool screenCenter = false)
    {
        var targets = GameManager.Instance.FieldData.Enemys
            .Where(e => e.IsVisible)
            .Where(e => e.IsDeath == false)
            .Where(e => Vector3.Distance(_parameter.FollowTarget.position, e.transform.position) < dis)
            .OfType<ITargetable>();

        if (disCenter)
        {
            targets = targets.OrderBy(e => Vector3.Distance(_parameter.FollowTarget.position, e.TargetTransform.position));
        }
        if (screenCenter)
        {
            targets = targets.OrderBy(e => Vector2.Distance(new Vector2(
            Screen.width / 2.0f, Screen.height / 2.0f), Camera.main.WorldToScreenPoint(e.TargetTransform.position)));
        }
        return targets.FirstOrDefault();
    }

    public void LockOn(bool lockOn,float dis = 20.0f, bool disCenter = false, bool screenCenter = false)
    {
        if (lockOn)
        {
            _target = FindTarget(dis,disCenter,screenCenter);
            GameManager.Instance.LockOnTarget = _target.TargetTransform;
            UiManager.Instance.ReceiveData("gameUi", new LockOnEventHandler(true, _target.TargetTransform.transform));
            IsLockOn = true;
            _camState = CamState.LockOn;
            return;
        }
        GameManager.Instance.LockOnTarget = null;
        UiManager.Instance.ReceiveData("gameUi", new LockOnEventHandler(false));
        IsLockOn = false;
        _camState = CamState.Control;
    }

    /// <summary>
    /// äpìxêßå¿óp
    /// </summary>
    /// <param name="angle">äpìx</param>
    /// <param name="min">ç≈è¨íl</param>
    /// <param name="max">ç≈ëÂíl</param>
    /// <returns></returns>
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
