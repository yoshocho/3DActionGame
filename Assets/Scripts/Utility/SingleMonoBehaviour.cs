using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMonoBehaviour<TOwer> : MonoBehaviour where TOwer :SingleMonoBehaviour<TOwer>
{
    private static TOwer m_instance = null;

    public static TOwer Instance
    {
        get 
        {
            if (m_instance == null)
            {
                Initialize();
            }
            return m_instance;
        } 
    }

    static void Initialize()
    {

        if (m_instance == null)
        {
            var previous = FindObjectOfType(typeof(TOwer));
            if (previous)
            {
                Debug.Log(string.Format("ヒエラルキーにある{0}をインスタンスに変換します", previous));
                m_instance = previous as TOwer;
            }
            else
            {
                Debug.LogWarning(string.Format("Hierarchy上に{0}が見つからなかったので生成します", typeof(TOwer).Name));
                var go = new GameObject(typeof(TOwer).Name);
                m_instance = go.AddComponent<TOwer>();

                m_instance.ForcedRun();
            }
        }
    }

    private void Awake()
    {
        CheckInstance();
    }

    bool CheckInstance()
    {
        if (m_instance == null)
        {
            m_instance = this as TOwer;
            OnAwake();
            return true;
        }
        else if (m_instance == this)
            return true;

        Destroy(this);
        return false;
    }


    protected virtual void ForcedRun() { }
    
    public static bool IsAlive => Instance != null;

    protected void DontDestroyObject()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnAwake(){ }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            OnRelease();
            m_instance = null;
            Debug.LogWarning(string.Format("{0}のインスタンスを破棄しました。",typeof(TOwer)));
        }
    }
    protected virtual void OnRelease() { }
}
