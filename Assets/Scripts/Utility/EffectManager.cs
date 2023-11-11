using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameUtility;

/// <summary>
/// 攻撃エフェクトなどの管理クラス
/// </summary>
public class EffectManager : MonoBehaviour,IManager
{
    [System.Serializable]
    public class Effect
    {
        public string Name = "";
        public GameObject EfObject = null;
    }

    public static EffectManager Instance { get; private set; } = null;

    [SerializeField]
    int _priority;
    public int Priority => _priority;

    Dictionary<string, GameObject> _effectDic = new Dictionary<string, GameObject>();

    [SerializeField]
    List<Effect> _effects = new List<Effect>();
    [SerializeField]
    DamageUi _damageUi;

    float _stopTime = default;
    float _frameTimer = default;
    float _timeScale = default;
    bool _isHitStop = default;

    private void Awake()
    {
        Instance = this;
        _timeScale = Time.timeScale;
        _effects.ForEach(ef => _effectDic.Add(ef.Name,ef.EfObject));
    }
    public void SetUp()
    {
        ServiceLocator<EffectManager>.Register(this);
    }

    private void Update()
    {
        if (_isHitStop && Time.timeScale == _timeScale)
        {
            _frameTimer = 0;
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            _isHitStop = false;
        }
        if (Time.timeScale == 0 && _frameTimer < _stopTime)
        {
            _frameTimer += Time.unscaledDeltaTime;
            if (_frameTimer >= _stopTime)
            {
                Time.timeScale = _timeScale;
            }
        }
    }

    public void HitStop(float power)
    {
        _stopTime = power * 1.0f / 24.0f;
        _isHitStop = true;
    }

    public GameObject PlayEffect(string key, Vector3 pos,Quaternion rot = default)
    {
        if (!Instance._effectDic.ContainsKey(key)) return null;
        return Instantiate(Instance._effectDic[key],pos,rot);
    }

    public GameObject PlayEffect(GameObject effectObj,Vector3 pos,Quaternion rot = default)
    {
        return Instantiate(effectObj,pos,rot);
    }

    public void DamageUiPop(GameObject target, int damage,Color color)
    {
        var go = Instantiate(Instance._damageUi.gameObject, Instance.transform.parent);
        var dmg = go.GetComponent<DamageUi>();
        dmg.Set(target,damage,Color.black);
    }

    public IEnumerator ControllerShake(Vector2 shakeVec, float shakeTime)
    {
        if (Gamepad.current == null) yield break;

        var gamePad = Gamepad.current;
        gamePad.SetMotorSpeeds(shakeVec.x, shakeVec.y);
        yield return new WaitForSeconds(shakeTime);
        gamePad.SetMotorSpeeds(default,default);
    }
}
