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
    CinemachineImpulseSource _impulseSource;

    bool _isLookAt = false;
    GameObject _lockOnTarget;
    
    float _defaultDistance = default;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _followTarget = GameObject.FindGameObjectWithTag("Player");
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _freeCam = GetComponentInChildren<CinemachineFreeLook>();
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

    void SetTarget(GameObject target)
    {
        _lockOnTarget = target;
    }

    void LookAtTarget(Vector3 target)
    {
        float cameraHeight = _freeCam.transform.position.y;
        Vector3 followPos = new Vector3(_freeCam.Follow.position.x, cameraHeight, _freeCam.Follow.position.z);
        Vector3 targetPos = new Vector3(target.x,cameraHeight,target.z);

    }
}
