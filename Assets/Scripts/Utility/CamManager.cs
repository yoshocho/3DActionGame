using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField]
    float _verticalSensitivity = 0.5f;
    [SerializeField]
    float _horizontalSensitivity = 0.5f;
    [SerializeField]
    float _targetDistance;

    Vector3 _planarDirection;
    [SerializeField]
    Transform _testTarget;

    
    [SerializeField]
    CamState _camState;
    float _horizontalAngle = 0.0f;
    float _verticalAngle = 10.0f;
    PlayerInput _input;
    private void Start()
    {
        _input = InputManager.Instance.PlayerInput;

        _camState = CamState.Control;
        _verticalAngle = _parameter.Angles.y;
        _horizontalAngle = _parameter.Angles.x;
    }

    //private void LateUpdate()
    //{
    //    ApplyCam();
    //}

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
                LockOn();
                break;
            default:
                break;
        }

        ApplyCam();
    }

    void LockOn()
    {
        Vector3 camToTarget = _testTarget.position - _cam.transform.position;
        Vector3 planarCamToTarget = Vector3.ProjectOnPlane(camToTarget,Vector3.up);
        Quaternion lookRotation = Quaternion.LookRotation(camToTarget,Vector3.up);
        
        _planarDirection = planarCamToTarget != Vector3.zero ? planarCamToTarget.normalized : _planarDirection; 
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
        Vector2 axis = _input.Player.CameraAxis.ReadValue<Vector2>();

        //Debug.Log(axis.x + ":" + axis.y);

        _horizontalAngle += axis.x * _horizontalSensitivity;
        _verticalAngle -= axis.y * _verticalSensitivity;

        _verticalAngle = ClampAngle(_verticalAngle, _verticalAngleMinLimit, _verticalAngleMaxLimit);

        _parameter.Angles = new Vector3(_verticalAngle, _horizontalAngle);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
