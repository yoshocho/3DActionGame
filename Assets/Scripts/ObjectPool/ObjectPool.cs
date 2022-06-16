using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolObject
{
    private T _origin;
    private Transform _poolParent;
    private List<T> _objcts = new List<T>();


    public void SetUp(T poolTarget,int count = 10,Transform parent = null)
    {
        _origin = poolTarget;
        _objcts = new List<T>();
        if (parent == null) _poolParent = new GameObject(poolTarget.name + " Pool").transform;
        else _poolParent = parent;

        for (int i = 0; i < count; i++)
        {
            var newObj = Object.Instantiate(_origin, _poolParent);
            newObj.gameObject.SetActive(false);
            newObj.SetUp();
            _objcts.Add(newObj);
        }
    }

    public T Get()
    {
        foreach (var obj in _objcts) 
        {
            if (obj.gameObject.activeSelf) continue;
            obj.gameObject.SetActive(true);
            obj.SetUp();
            return obj;
        }

        var newObj = Object.Instantiate(_origin, _poolParent);
        newObj.SetUp();
        _objcts.Add(newObj);
        return newObj;
    }

    public void Clear()
    {
        foreach(var obj in _objcts) 
        {
            Object.Destroy(obj.gameObject);
            _objcts.Clear();
        }
    }
}
