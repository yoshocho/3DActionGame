using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(GroundChecker))]
public class RigidMover : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 5.0f;
    [SerializeField]
    float _gravityScale = 0.98f;
    [SerializeField]
    float _rotateSpeed = 10.0f;
    Rigidbody _rb;
    Vector3 _velocity = default;
    Quaternion _targetRot;
    [SerializeField]
    bool _useGravity = true;
    Transform _selfTans;

    GroundChecker _groundChecker;

    public bool IsControl { get; set; } = false;

    public Rigidbody Rb => _rb;
    public bool UseGravity { set { _useGravity = value; } }
    public Vector3 Velocity { get => _velocity; set {
            if (IsControl) return;
            _velocity = value;
        } }
    public float SetMoveSpeed { set { _moveSpeed = value; } }
    public Quaternion SetRot { set { _targetRot = value; } }
    public float SetRotateSpeed { set { _rotateSpeed = value; } }
    public void SetUp(Transform userTrans)
    {
        _selfTans = userTrans;
        _rb = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();
    }
    private void Reset()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rb.useGravity = false;
    }
    private void FixedUpdate()
    {
        if (IsControl) return;

        ApplyRotate();
        if (_useGravity) ApplyGravity();
        ApplyMove();
    }
    void ApplyMove()
    {
        var velo = Vector3.Scale(_velocity, new Vector3(_moveSpeed, 1.0f, _moveSpeed));
        _rb.velocity = velo;
    }
    void ApplyRotate()
    {
        var rot = _selfTans.rotation;
        rot = Quaternion.Slerp(rot, _targetRot, _rotateSpeed * Time.deltaTime);
        transform.rotation = rot;
    }
    void ApplyGravity()
    {
        if (!IsGround())
        {
            _velocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _velocity.y = 0.0f;
        }
    }
    public bool IsGround()
    {
        return _groundChecker.IsGround();
    }

    public void SetPosition(Vector3 pos)
    {
        _rb.position = pos;
    }
}
