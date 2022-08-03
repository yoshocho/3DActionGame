using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;
using System.Linq;

[System.Serializable]
public class StyleAnim 
{
    public NewPlayer.StateEvent PlayerState;
    public AnimClip AnimClip;
}

[CreateAssetMenu(fileName = "StyleData", menuName = "ScriptableObjects/StyleData")]
public class StyleData : ScriptableObject
{
    [SerializeField]
    public WeaponType WeaponType;
    [SerializeField]
    public List<AttackData> AttackData = new List<AttackData>();
    [SerializeField]
    public AnimatorOverrideController OverrideAnimator;
}