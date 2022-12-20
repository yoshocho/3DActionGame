using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSetting;

public interface IDamage
{
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ数</param>
    void AddDamage(int damage);
}