using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharaConMover : MonoBehaviour
{
    [SerializeField]
    float _gravityScale;
    [SerializeField]
    float _rotateSpeed;
    CharacterController _controller;
    Vector3 _velocity;
    Quaternion _targetRot;

    public bool UseGravity { get; set; } = true;
    public Vector3 SetVelocity { set { _velocity = value; } }
    public Quaternion SetRot { set { _targetRot = value; } }
    public float SetRotateSpeed { set { _rotateSpeed = value; } }
    public void SetUp()
    {
        TryGetComponent(out _controller);
    }
    private void FixedUpdate()
    {
        ApplyMove();
        ApplyRotate();
        if(UseGravity) ApplyGravity();
    }
    void ApplyMove()
    {
        _controller.Move(Time.deltaTime * _velocity);
    }
    void ApplyRotate()
    {
        var rot = transform.rotation;
        rot = Quaternion.Slerp(rot, _targetRot, _rotateSpeed * Time.deltaTime);
        transform.rotation = rot;
    }
    void ApplyGravity()
    {
        _velocity.y += _gravityScale * Physics.gravity.y * Time.deltaTime;
    }
}
