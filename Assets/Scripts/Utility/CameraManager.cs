using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx;
using System;
using System.Linq;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } = default;

    [SerializeField]
    GameObject _followTarget;
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
    
    float _defaultDistance = default;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _followTarget = GameObject.FindGameObjectWithTag("Player");
        _camDistance = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        m_camPov = _vCam.GetCinemachineComponent<CinemachinePOV>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _camDistance.m_CameraDistance = _cameraDistance;
        _defaultDistance = _camDistance.m_CameraDistance;
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

    public GameObject FindTarget(Transform userTrans ,float dis,bool screenCenter = false)
    {
        var enemys = GameManager.Instance.FieldData.Enemys
            .Where(e => e.IsVisible)
            .Where(e => !e.IsDeath)
            .Where(e => Vector3.Distance(e.transform.position, userTrans.position) < dis);

         enemys.OrderBy(e =>  Vector3.Distance(userTrans.position ,e.transform.position));
        if (screenCenter) enemys.OrderBy(e => Vector2.Distance(new Vector2(
            Screen.width /2.0f,Screen.height / 2.0f),Camera.main.WorldToScreenPoint(e.transform.position)));

        return enemys.FirstOrDefault().gameObject;
    }
    //void Update()
    //{
    //    //float scroll = Input.mouseScrollDelta.y;
    //    //m_camDistance.m_CameraDistance -= scroll * m_zoomSpeed;
    //    float scrollSpeed = Mathf.Clamp(m_camDistance.m_CameraDistance, m_scrollMin, m_scrollMax);
    //    m_camDistance.m_CameraDistance = scrollSpeed;  
    //}
}
