using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System.Linq;

[System.Serializable]
public class Weapon 
{
    public WeaponType Type;
    public GameObject WeaponObj;
}

public class WeaponHolder : MonoBehaviour
{
   
    [SerializeField]
    Transform[] _holderPoss;
    [SerializeField]
    Transform _handTrans = default;
    [SerializeField]
    List<Weapon> _weapons = new List<Weapon>();
    GameObject _currentWeapon = default;

    WeaponType _currentType;
    
    public void SetWeapon(WeaponType type)
    {
        if (_currentWeapon != null) ResetHolder();
        _currentType = type;
        var weapon = _weapons.First(w => w.Type == type);
        _currentWeapon = weapon.WeaponObj;
        
        Debug.Log(string.Format(" 武器を変更{0}",type));
        
        SetPosition(weapon.WeaponObj.transform, _handTrans);
    }
    public void ResetHolder()
    {
        switch (_currentType)
        {
            case WeaponType.Hand:
                break;
            case WeaponType.LightSword:
                SetPosition(_currentWeapon.transform, _holderPoss[0]);
                break;
            case WeaponType.HeavySword:
                SetPosition(_currentWeapon.transform, _holderPoss[1]);
                break;
            default:
                break;
        }
        _currentWeapon = null;
    }

    void SetPosition(Transform parent, Transform to)
    {
        parent.SetParent(to);
        parent.localPosition = Vector3.zero;
        parent.localRotation = Quaternion.identity;
    }
}
