using UnityEngine;

public class CameraShake : IAttackEffect
{
    [Header("—h‚ê‚ÌU‚ê•")]
    [SerializeField]
    float _width = 0.2f;
    [Header("—h‚ê‚Ì‰ñ”")]
    [SerializeField, Range(0, 10)]
    int _count = 2;
    [Header("—h‚ê‚ÌŠÔŠu")]
    [SerializeField, Range(0.0f, 1.0f)]
    float _duration = 0.2f;
    public void SetUp(Transform ownerTrans) { }
    public void SetEffect()
    {
        CamManager.Instance.Shake(_width, _count, _duration);
    }
}