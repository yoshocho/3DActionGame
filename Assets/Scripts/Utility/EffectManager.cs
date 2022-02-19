using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [System.Serializable]
    public class Effect 
    {
        public string Name = "";
        public GameObject EfObject = null;
    }

    public static EffectManager Instance { get; private set; } = null;
    Dictionary<string, GameObject> m_effectDic = new Dictionary<string, GameObject>();

    [SerializeField] List<Effect> m_effects = new List<Effect>();

    private void Awake()
    {
        Instance = this;
        m_effects.ForEach(ef => m_effectDic.Add(ef.Name,ef.EfObject));
    }

    static public GameObject PlayEffect(string key, Vector3 pos,Quaternion rot = default)
    {
        if (!Instance.m_effectDic.ContainsKey(key)) return null;
        return Instantiate(Instance.m_effectDic[key],pos,rot);
    }
}
