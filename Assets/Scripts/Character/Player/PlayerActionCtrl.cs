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
        _animCtrl = GetComponent<AnimationCtrl>();
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

        _animCtrl
            .ChangeClip("Run", style.GetStyleAnim(PlayerStateMachine.StateEvent.Run))
            .ChangeClip("Sprint", style.GetStyleAnim(PlayerStateMachine.StateEvent.Sprint))
            .ChangeClip("Idle", style.GetStyleAnim(PlayerStateMachine.StateEvent.Idle))
            .ChangeClip("Avoid", style.GetStyleAnim(PlayerStateMachine.StateEvent.Avoid))
            .ChangeClip("Jump", style.GetStyleAnim(PlayerStateMachine.StateEvent.Jump))
            .ChangeClip("Fall", style.GetStyleAnim(PlayerStateMachine.StateEvent.Fall))
            .ChangeClip("Land", style.GetStyleAnim(PlayerStateMachine.StateEvent.Land))
            .ChangeClip("RunEnd",style.Clips[0])       // ステート外でのアニメーションは直接index指定 *修正予定 
            .ChangeClip("SprintEnd",style.Clips[1])
            .ChangeClip("AirDush",style.Clips[2]);

    }
}
