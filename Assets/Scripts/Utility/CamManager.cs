using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static CameraManager Instance { get; private set; } = null;

    [SerializeField]
    Transform _parent;
    [SerializeField]
    Transform _child;
    [SerializeField]
    Camera _cam;
    [SerializeField]
    CameraParameter _parameter;
    [SerializeField]
    float _dampingValue = 6.0f;
    [SerializeField]
    float _verticalSensitivity;
    [SerializeField]
    float _horizontalSensitivity;

    public CameraParameter Parameter { get { return _parameter;}set { _parameter = value; } }

    public void SetUp()
    {

    }

    private void FixedUpdate()
    {
        ApplyCam();
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

    void ApplyAxis()
    {

    }
}
