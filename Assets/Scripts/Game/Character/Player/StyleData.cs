using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System.Linq;

public enum PlayerAnimState
{
    Idle,
    Run,
    RunEnd,
    Sprint,
    SprintEnd,
    Avoid,
    AirDush,
    Jump,
    Fall,
    Land,

}

[System.Serializable]
public class StyleAnim 
{
    public PlayerAnimState AnimState;
    public AnimationClip AnimClip;
}

[CreateAssetMenu(fileName = "StyleData", menuName = "ScriptableObjects/StyleData")]
public class StyleData : ScriptableObject
{
    [SerializeField]
    public WeaponType WeaponType;
    [SerializeField]
    public List<AttackList> AttackData = new List<AttackList>();
    [SerializeField]
    public List<StyleAnim> StyleAnimData = new List<StyleAnim>();

    public AnimationClip GetStyleAnim(PlayerAnimState state)
    {
        return StyleAnimData.FirstOrDefault(data => data.AnimState == state).AnimClip;
    }
}
