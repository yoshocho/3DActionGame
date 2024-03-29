using System.Linq;
using UnityEngine;
using DG.Tweening;

//[ExecuteInEditMode]
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
    public bool Damping { get; set; } = true;
    [SerializeField]
    float _verticalAngleMinLimit = -30.0f;
    [SerializeField]
    float _verticalAngleMaxLimit = 50;

    [SerializeField, Header("水平感度")]
    float _horizontalSensitivity = 0.5f;
    [SerializeField, Header("垂直感度")]
    float _verticalSensitivity = 0.5f;
    [SerializeField, Header("水平操作逆転")]
    bool _invartX = false;
    [SerializeField, Header("垂直操作逆転")]
    bool _invartY = false;


    [SerializeField]
    float _defaultVerticalAngle = 20.0f;
    [SerializeField]
    float _ctrlRotationSharpness = 12.0f;
    [SerializeField]
    float _lockOnRotationSharpness = 9.0f;
    float _rotationSharpness;

    #region LockOn
    public bool IsLockOn { get; private set; }
    Quaternion _targetRot;
    Quaternion _newRotation;
    float _targetVerticalAngle;
    Vector3 _planarDirection;
    ITargetable _target;
    float _framingLerp;
    public ITargetable Target { get => _target; }
    #endregion

    float _inputX;
    float _inputY;

    [SerializeField]
    CamState _camState;
    IInputProvider _inputProvider;

    private void Awake()
    {
        Instance = this;
        ServiceLocator<CamManager>.Register(Instance);
    }

    private void Start()
    {
        SetUp();
    }
    void SetUp()
    {
        _planarDirection = _parameter.FollowTarget.transform.forward;
        _targetVerticalAngle = _defaultVerticalAngle;
        _targetRot = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0.0f, 0.0f);
        _inputProvider = ServiceLocator<IInputProvider>.Instance;
    }

    //デバッグ用
    //private void LateUpdate()
    //{
    //    ApplyCam();
    //}

    private void FixedUpdate()
    {
        if (_inputProvider == null) return;
        Vector2 inputAxis = _inputProvider.GetCameraAxis();

        _inputX = inputAxis.x * _horizontalSensitivity;
        _inputY = inputAxis.y * _verticalSensitivity;

        if (_invartX) _inputX *= -1;
        if (_invartY) _inputY *= -1;

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
        if (_target == null || _target.TargetTransform == null) LockOn(false);

        Vector3 camToTarget = _target.TargetTransform.position - _cam.transform.position;
        Vector3 planarCamToTarget = Vector3.ProjectOnPlane(camToTarget, Vector3.up);
        Quaternion lookRot = Quaternion.LookRotation(camToTarget,Vector3.up);

        _framingLerp = Mathf.Clamp01(_framingLerp + Time.deltaTime * 4);

        _planarDirection = planarCamToTarget != Vector3.zero ? planarCamToTarget.normalized : _planarDirection;

        //_targetVerticalAngle = ClampAngle(lookRot.eulerAngles.x, _verticalAngleMinLimit, _verticalAngleMaxLimit);

        _targetVerticalAngle = lookRot.eulerAngles.x;
        if(_targetVerticalAngle > 180) _targetVerticalAngle = _targetVerticalAngle - 360;
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
            _parameter.Position = Damping ? Vector3.Lerp(_parameter.Position,
                _parameter.FollowTarget.position, Time.deltaTime * _dampingValue)
                : _parameter.FollowTarget.position;
        }
        _parent.position = _parameter.Position;

        _targetRot = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0.0f, 0.0f);
        _newRotation = Quaternion.Slerp(_cam.transform.rotation, _targetRot, Time.deltaTime * 9.0f); //＊マジックナンバー
        _parameter.Angles = _newRotation.eulerAngles;
    }

    void ControlCam()
    {
        _planarDirection = Quaternion.Euler(0.0f, _inputX, 0.0f) * _planarDirection;
        _targetVerticalAngle = ClampAngle(_targetVerticalAngle + _inputY, _verticalAngleMinLimit, _verticalAngleMaxLimit);
    }


    public ITargetable FindTarget(float dis, bool disCenter = false, bool screenCenter = false)
    {
        var targets = GameManager.Instance.FieldData.Enemys
            .Where(e => e.IsVisible)
            .Where(e => e.IsDeath == false)
            .Where(e => Vector3.Distance(_parameter.FollowTarget.position, e.transform.position) < dis)
            .OfType<ITargetable>()
            .Where(t => t.Targetable);

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

    public void Shake(float width,int count,float duration)
    {
        var camTrans = _cam.transform;
        var seq = DOTween.Sequence();

        var partDuration = duration / count / 2.0f;
        var widthHalf = width / 2.0f;
        //Vector2 widthHalf = new Vector2(widthHalf.x / 2.0);

        for (int i = 0; i < count - 1; i++)
        {
            seq.Append(camTrans.DOLocalRotate(new Vector3(-widthHalf,0.0f),partDuration));
            seq.Append(camTrans.DOLocalRotate(new Vector3(widthHalf, 0.0f), partDuration));
        }
        seq.Append(camTrans.DOLocalRotate(new Vector3(-widthHalf, 0.0f), partDuration));
        seq.Append(camTrans.DOLocalRotate(Vector3.zero, partDuration));
    }

    public void LockOn(bool lockOn, float dis = 20.0f, bool disCenter = false, bool screenCenter = false)
    {
        if (lockOn)
        {
            _target = FindTarget(dis, disCenter, screenCenter);
            GameManager.Instance.LockOnTarget = _target.TargetTransform;
            ServiceLocator<UiManager>.Instance.ReceiveData("gameUi",
                new LockOnEventHandler(true, _target.TargetTransform.transform));

            IsLockOn = true;
            _camState = CamState.LockOn;
            return;
        }
        GameManager.Instance.LockOnTarget = null;
        ServiceLocator<UiManager>.Instance.ReceiveData("gameUi", new LockOnEventHandler(false));
        IsLockOn = false;
        _camState = CamState.Control;
    }

    /// <summary>
    /// 角度制限用
    /// </summary>
    /// <param name="angle">角度</param>
    /// <param name="min">最小値</param>
    /// <param name="max">最大値</param>
    /// <returns></returns>
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
