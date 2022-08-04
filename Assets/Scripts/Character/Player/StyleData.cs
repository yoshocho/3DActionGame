using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System.Linq;

[System.Serializable]
public class StyleAnim 
{
    public PlayerStateMachine.StateEvent PlayerState;
    public AnimationClip AnimClip;
}

[CreateAssetMenu(fileName = "StyleData", menuName = "ScriptableObjects/StyleData")]
public class StyleData : ScriptableObject
{
    [SerializeField]
    public WeaponType WeaponType;
    [SerializeField]
    public List<AttackData> AttackData = new List<AttackData>();
    [SerializeField]
    public List<StyleAnim> StyleAnimData = new List<StyleAnim>();
    
    public AnimationClip GetStyleAnim(PlayerStateMachine.StateEvent state)
    {
        return StyleAnimData.FirstOrDefault(data => data.PlayerState == state).AnimClip;
    }
}
