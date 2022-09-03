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
    GroundChecker _groundChecker;
    Rigidbody _rb;
    Vector3 _velocity = default;
    Quaternion _targetRot;
    [SerializeField]
    bool _useGravity = true;

    public bool UseGravity { set { _useGravity = value; } }
    public Vector3 Velocity { get => _velocity; set { _velocity = value; } }
    public float SetMoveSpeed { set { _moveSpeed = value; } }
    public Quaternion SetRot { set { _targetRot = value; } }
    public float SetRotateSpeed { set { _rotateSpeed = value; } }
    public void SetUp()
    {
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
        var rot = transform.rotation;
        rot = Quaternion.Slerp(rot, _targetRot, _rotateSpeed * Time.deltaTime);
        transform.rotation = rot;
    }
    void ApplyGravity()
    {
        if (!IsGround())
        {
            _velocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
        }
    }
    public bool IsGround()
    {
        return _groundChecker.IsGround();
    }
}
