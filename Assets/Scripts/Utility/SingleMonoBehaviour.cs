using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMonoBehaviour<TOwer> : MonoBehaviour where TOwer :SingleMonoBehaviour<TOwer>
{
    private static TOwer s_instance = null;

    public static TOwer Instance
    {
        get 
        {
            if (s_instance == null)
            {
                Initialize();
            }
            return s_instance;
        } 
    }

    static void Initialize()
    {

        if (s_instance == null)
        {
            var previous = FindObjectOfType(typeof(TOwer));
            if (previous)
            {
                Debug.Log(string.Format("ヒエラルキーにある{0}をインスタンスに変換します", previous));
                s_instance = previous as TOwer;
            }
            else
            {
                Debug.LogWarning(string.Format("Hierarchy上に{0}が見つからなかったので生成します", typeof(TOwer).Name));
                var go = new GameObject(typeof(TOwer).Name);
                s_instance = go.AddComponent<TOwer>();

                s_instance.ForcedRun();
            }
        }
    }

    private void Awake()
    {
        CheckInstance();
    }

    bool CheckInstance()
    {
        if (s_instance == null)
        {
            s_instance = this as TOwer;
            OnAwake();
            return true;
        }
        else if (s_instance == this)
            return true;

        Destroy(this);
        return false;
    }


    protected virtual void ForcedRun() { }
    
    public static bool IsAlive => Instance != null;

    /// <summary>
    /// 駐屯オブジェクトにする
    /// </summary>
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
            s_instance = null;
            Debug.LogWarning(string.Format("{0}のインスタンスを破棄しました。",typeof(TOwer)));
        }
    }
    /// <summary>
    /// オブジェクトが破棄された時に呼ばれる
    /// </summary>
    protected virtual void OnRelease() { }
}
