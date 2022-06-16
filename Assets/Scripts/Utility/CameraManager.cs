using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UniRx;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } = default;

    [SerializeField]
    CinemachineVirtualCamera _vCam = default;
    [SerializeField]
    CinemachineFreeLook _freeCam = default;
    [SerializeField]
    float _zoomSpeed = 1;
    [SerializeField]
    private float _cameraDistance = 8.5f;
    [SerializeField]
    private float _scrollMin = 3f;
    [SerializeField]
    float _zoomDistance = 4f;
    CinemachineFramingTransposer _camDistance;
    CinemachinePOV m_camPov;
    CinemachineImpulseSource _impulseSource;
    
    bool _isJust = default;
    float _defaultDistance = default;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _camDistance = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        m_camPov = _vCam.GetCinemachineComponent<CinemachinePOV>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _camDistance.m_CameraDistance = _cameraDistance;
        _defaultDistance = _camDistance.m_CameraDistance;

        BulletTimeManager.Instance.JustSuccess
            .Subscribe(_ => StartCoroutine(OnjustCam()))
            .AddTo(this);
    } 

    IEnumerator OnjustCam()
    {
        Debug.Log("OnZoom");
        while (_camDistance.m_CameraDistance > _zoomDistance)
        {
            _camDistance.m_CameraDistance -= 6f * Time.unscaledDeltaTime;
            yield return null;
        }
        Debug.Log("ZoomOut");
        _isJust = true;
        yield return new WaitUntil(() => _isJust);
        while (_camDistance.m_CameraDistance <= _defaultDistance)
        {
            _camDistance.m_CameraDistance += 1f;
        }

        _isJust = false;
    }

    public static IEnumerator ZoomIn(float zoomSpeed = 0.5f, Action zoomEnd = null)
    {
        yield return null;
    }

    public static IEnumerator ZoomOut(float zoomSpeed = 0.5f, Action zoomEnd = null)
    {
        yield return null;
    }

    public static void ShakeCam(Vector3 vec = default)
    {
        Instance._impulseSource.GenerateImpulse(vec);
    }

    //void Update()
    //{
    //    //float scroll = Input.mouseScrollDelta.y;
    //    //m_camDistance.m_CameraDistance -= scroll * m_zoomSpeed;
    //    float scrollSpeed = Mathf.Clamp(m_camDistance.m_CameraDistance, m_scrollMin, m_scrollMax);
    //    m_camDistance.m_CameraDistance = scrollSpeed;  
    //}
}
