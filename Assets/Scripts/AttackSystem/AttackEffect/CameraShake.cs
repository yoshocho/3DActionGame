using UnityEngine;

public class CameraShake : IAttackEffect
{
    [Header("�h��̐U�ꕝ")]
    [SerializeField]
    float _width = 0.2f;
    [Header("�h��̉�")]
    [SerializeField, Range(0, 10)]
    int _count = 2;
    [Header("�h��̊Ԋu")]
    [SerializeField, Range(0.0f, 1.0f)]
    float _duration = 0.2f;
    public void SetUp(Transform ownerTrans) { }
    public void SetEffect()
    {
        CamManager.Instance.Shake(_width, _count, _duration);
    }
}