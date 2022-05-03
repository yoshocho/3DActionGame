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
    CinemachineVirtualCamera m_cam = default;
    [SerializeField]
    CinemachineFreeLook m_freeCam = default;
    [SerializeField]
    float m_zoomSpeed = 1;
    [SerializeField]
    private float m_cameraDistance = 8.5f;
    [SerializeField]
    private float m_scrollMin = 3f;
    [SerializeField]
    float m_zoomDistance = 4f;
    CinemachineFramingTransposer m_camDistance;
    CinemachinePOV m_camPov;
    CinemachineImpulseSource m_impulseSource;

    bool m_isJust = default;
    float m_defaultDistance = default;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_camDistance = m_cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        m_camPov = m_cam.GetCinemachineComponent<CinemachinePOV>();
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
        m_camDistance.m_CameraDistance = m_cameraDistance;
        m_defaultDistance = m_camDistance.m_CameraDistance;

        BulletTimeManager.Instance.JustSuccess
            .Subscribe(_ => StartCoroutine(OnjustCam()))
            .AddTo(this);
    }
    IEnumerator OnjustCam()
    {
        Debug.Log("OnZoom");
        while (m_camDistance.m_CameraDistance > m_zoomDistance)
        {
            m_camDistance.m_CameraDistance -= 6f * Time.unscaledDeltaTime;
            yield return null;
        }
        Debug.Log("ZoomOut");
        m_isJust = true;
        yield return new WaitUntil(() => m_isJust);
        while (m_camDistance.m_CameraDistance <= m_defaultDistance)
        {
            m_camDistance.m_CameraDistance += 1f;
        }

        m_isJust = false;
    }

    public static IEnumerator ZoomIn() { yield return null; }

    public static IEnumerator ZoomOut() { yield return null; }

    public static void ShakeCam()
    {
         Instance.m_impulseSource.GenerateImpulse();
    }

    //void Update()
    //{
    //    //float scroll = Input.mouseScrollDelta.y;
    //    //m_camDistance.m_CameraDistance -= scroll * m_zoomSpeed;
    //    float scrollSpeed = Mathf.Clamp(m_camDistance.m_CameraDistance, m_scrollMin, m_scrollMax);
    //    m_camDistance.m_CameraDistance = scrollSpeed;  
    //}
}
