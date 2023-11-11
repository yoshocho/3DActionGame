using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System.Linq;

public class PlayerActionCtrl : MonoBehaviour
{
    
    [SerializeField]
    List<StyleData> _styleDatas = new List<StyleData>();
    public List<StyleData> StyleDatas => _styleDatas;

    public WeaponType CurrentType { get; private set; }

    ActionCtrl _actCtrl;
    AnimationCtrl _animCtrl;
    WeaponHolder _weaponHolder;

    public void SetUp()
    {
        _actCtrl = GetComponent<ActionCtrl>();
        _animCtrl = GetComponentInChildren<AnimationCtrl>();
        _weaponHolder = GetComponent<WeaponHolder>();
    }

    public void SetStyle(WeaponType type)
    {
        CurrentType = type;
        var style = _styleDatas.FirstOrDefault(s => s.WeaponType == type);

        if (style == null)
        {
            Debug.LogWarning(string.Format("指定されたデータはありません{0}", type));
            return;
        }

        _actCtrl.AttackDatas = style.AttackData;
        _weaponHolder.SetWeapon(style.WeaponType);

        foreach (var item in style.StyleAnimData)
        {
            _animCtrl.ChangeClip(item.AnimState.ToString(), style.GetStyleAnim(item.AnimState));
        }
    }
}
