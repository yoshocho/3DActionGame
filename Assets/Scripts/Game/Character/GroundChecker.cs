using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Tooltip("�ڒn����ł̒��S����̋���")]
    [SerializeField] float _isGroundLength = 1.05f;
    [Tooltip("�ڒn����͈̔�")]
    [SerializeField] float _isGroundRadius = 0.18f;
    [Tooltip("�n�ʂ̃��C���[")]
    [SerializeField] LayerMask _groundLayer;

    public bool IsGround()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        Ray ray = new Ray(start, Vector3.down);
        bool isGround = Physics.SphereCast(ray, _isGroundRadius, _isGroundLength, _groundLayer);
        return isGround;
    }

    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmos()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        Vector3 end = start + Vector3.down * _isGroundLength;
        Color color = Color.magenta;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(end, _isGroundRadius);
    }
}
