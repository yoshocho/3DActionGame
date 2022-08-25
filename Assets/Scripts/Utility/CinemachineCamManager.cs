using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx;
using System;
using System.Linq;

public class CinemachineCamManager : MonoBehaviour
{
    public static CinemachineCamManager Instance { get; private set; } = default;

    [SerializeField]
    GameObject _followTarget;
    [SerializeField]
    Transform _testTarget;
    [SerializeField]
    CinemachineVirtualCamera _vCam = default;
    [SerializeField]
    CinemachineFreeLook _freeCam = default;

    CinemachineImpulseSource _impulseSource;
    float _defaultDistance = default;

    [SerializeField]
    float _blendTime = 0.2f;

    bool _isLookAt = false;
    float _lockOnAngle;
    float _lockOnProgess;
    float _lockOnVelocity;
    

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _freeCam = GetComponentInChildren<CinemachineFreeLook>();
        InputManager.Instance.PlayerInput.Player.LockOn.started += context =>
        LookAtTarget(_testTarget.position);
    }
    private void Update()
    {
        if (_isLookAt)
        {
            _freeCam.m_YAxis.Value = _testTarget.position.y;
            float angle = Mathf.SmoothDamp(_lockOnProgess, _lockOnAngle, ref _lockOnVelocity, _blendTime);
            _freeCam.m_XAxis.Value = angle - _lockOnProgess;
            
            _lockOnProgess = angle;

            if (Mathf.Abs(_lockOnProgess - _lockOnAngle) <= 0.01f)
            {
                _isLookAt = false;
            }

            _lockOnAngle = angle;
        }

    }

    public void ShakeCam(Vector3 vec = default)
    {
        Debug.Log("CameraShake" + vec);
        _impulseSource.GenerateImpulse(vec);
    }

    public GameObject FindTarget(Transform userTrans ,float dis,bool disCenter = false,bool screenCenter = false)
    {
        var targets = GameManager.Instance.FieldData.Enemys
            .Where(e => e.IsVisible)
            .Where(e => e.IsDeath == false)
            .Where(e => Vector3.Distance(userTrans.position, e.transform.position) < dis);

        if (disCenter)
        {
           targets = targets.OrderBy(e => Vector3.Distance(userTrans.position, e.transform.position));
        }
        if (screenCenter)
        {
            targets = targets.OrderBy(e => Vector2.Distance(new Vector2(
            Screen.width / 2.0f, Screen.height / 2.0f), Camera.main.WorldToScreenPoint(e.transform.position)));
        }
        return targets.FirstOrDefault().gameObject;
    }

    public void LookAtTarget(Vector3 target)
    {
        float cameraHeight = _freeCam.transform.position.y;
        Vector3 followPos = new Vector3(_freeCam.Follow.position.x, cameraHeight, _freeCam.Follow.position.z);
        Vector3 targetPos = new Vector3(target.x,cameraHeight,target.z);

        Vector3 followToTarget = targetPos - followPos;
        Vector3 followToTargetReverse = Vector3.Scale(followToTarget, -Vector3.one);
        Vector3 followToCamera = _freeCam.transform.position - followPos;

        Vector3 axis = Vector3.Cross(followToCamera,followToTargetReverse);
        float dir = axis.y < 0 ? -1 : 1;
        float angle = Vector3.Angle(followToCamera,followToTargetReverse);

        _lockOnAngle = angle * dir;
        _isLookAt = true;
        _lockOnVelocity = 0.0f;
        _lockOnProgess = 0.0f;
    }
}
