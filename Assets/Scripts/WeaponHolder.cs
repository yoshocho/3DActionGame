using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    HEAVY_SWORD,
    LIGHT_SWORD
}
public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    GameObject m_heavySwordHolder = default;

    [SerializeField]
    GameObject m_lightSwordHolder = default;

    [SerializeField]
    GameObject m_currentHolder = default;

    [SerializeField]
    List<GameObject> m_weaponList = new List<GameObject>();

    GameObject m_currentWeapon = default;

    public HitCtrl HitCtrl { get; private set; } = default;

    private void Start()
    {
        //SetWeapon(1);
    }

    public void ChangeWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.HEAVY_SWORD:
                SetWeapon(0);
                break;
            case WeaponType.LIGHT_SWORD:
                SetWeapon(1);
                break;
            default:
                break;
        }
    }

    void SetWeapon(int weaponId)
    {
        m_currentWeapon = m_weaponList[weaponId];
        HitCtrl = m_weaponList[weaponId].GetComponent<HitCtrl>();
        SetPosition(m_weaponList[weaponId].transform, m_currentHolder.transform);
    }
    public void ResetHolder()
    {
        
        SetPosition(m_currentWeapon.transform, m_heavySwordHolder.transform);
    }

    public void WeaponTriggerEnable()
    {
        this.HitCtrl.m_atkTrigeer.enabled = true;
    }
    public void WeaponTriggerDisable()
    {
        this.HitCtrl.m_atkTrigeer.enabled = false;
    }

    void SetPosition(Transform weapon,Transform to)
    {
        weapon.SetParent(to);
        weapon.localPosition = Vector3.zero;
        weapon.localRotation = Quaternion.identity;
    }
}
