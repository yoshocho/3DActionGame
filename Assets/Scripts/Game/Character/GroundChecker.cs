using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Tooltip("接地判定での中心からの距離")]
    [SerializeField] float _isGroundLength = 1.05f;
    [Tooltip("接地判定の範囲")]
    [SerializeField] float _isGroundRadius = 0.18f;
    [Tooltip("地面のレイヤー")]
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
