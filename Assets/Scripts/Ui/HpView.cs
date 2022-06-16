using UnityEngine;
using UnityEngine.UI;

public class HpView : MonoBehaviour
{
    [SerializeField]
    Slider _hpBar = null;
    int _maxHp;
    public void ApllyHpBar(int hp)
    {
        _hpBar.value = hp / (float)_maxHp;
    }
    public void SetUp(int hp)
    {
        _maxHp = hp;
    }
}
