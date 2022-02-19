using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ数</param>
    void AddDamage(int damage);
}
