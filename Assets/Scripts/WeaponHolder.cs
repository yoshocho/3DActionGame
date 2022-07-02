using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public enum WeaponType
{
    HEAVY,
    LIGHT,
}
public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    GameObject m_heavySwordHolder = default;

    [SerializeField]
    GameObject m_lightSwordHolder = default;


    [SerializeField]
    Transform[] _holderPos;
    [SerializeField]
    GameObject m_currentHolder = default;
    [SerializeField]
    List<GameObject> m_weaponList = new List<GameObject>();
    GameObject m_currentWeapon = default;
    AttackSetting.WeaponType _currentType;
    public HitCtrl HitCtrl { get; private set; } = default;

    private void Start()
    {
        //SetWeapon(1);
    }

    public void ChangeWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.HEAVY:
                if (m_currentWeapon)
                    SetPosition(m_currentWeapon.transform, m_lightSwordHolder.transform);
                SetWeapon(0);
                break;
            case WeaponType.LIGHT:
                if (m_currentWeapon)
                    SetPosition(m_currentWeapon.transform, m_heavySwordHolder.transform);
                SetWeapon(1);
                break;
            default:
                break;
        }
    }

    void SetWeapon(int weaponId)
    {
        Debug.Log("変更" + weaponId);
        m_currentWeapon = m_weaponList[weaponId];
        SetPosition(m_weaponList[weaponId].transform, m_currentHolder.transform);
    }
    public void ResetHolder()
    {
        //switch (HitCtrl.WeaponType)
        //{
        //    case WeaponType.HEAVY:
        //        SetPosition(m_currentWeapon.transform, m_heavySwordHolder.transform);
        //        break;
        //    case WeaponType.LIGHT:
        //        SetPosition(m_currentWeapon.transform, m_lightSwordHolder.transform);
        //        break;
        //    default:
        //        break;
        //}
        switch (_currentType)
        {
            case AttackSetting.WeaponType.Hand:
                break;
            case AttackSetting.WeaponType.LightSword:
                SetPosition(m_currentWeapon.transform, m_lightSwordHolder.transform);

                break;
            case AttackSetting.WeaponType.HeavySword:
                SetPosition(m_currentWeapon.transform, m_heavySwordHolder.transform);
                break;
            default:
                break;
        }

    }

    void SetPosition(Transform weapon, Transform to)
    {
        weapon.SetParent(to);
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.identity;
    }
}
