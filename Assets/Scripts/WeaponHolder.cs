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


    private void Start()
    {
        SetWeapon(0);
    }

    public void SetWeapon(int weaponId)
    {
        m_currentWeapon = m_weaponList[weaponId];
        m_weaponList[weaponId].transform.SetParent(m_currentHolder.transform);
        m_weaponList[weaponId].transform.localPosition = Vector3.zero;
        m_weaponList[weaponId].transform.localRotation = Quaternion.identity;   
    }
    public void ResetHolder()
    {
        m_currentWeapon.transform.SetParent(m_heavySwordHolder.transform);
        m_currentWeapon.transform.localPosition = Vector3.zero;
        m_currentWeapon.transform.localRotation = Quaternion.identity;
    }

}
