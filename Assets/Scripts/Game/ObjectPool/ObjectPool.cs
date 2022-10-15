using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// オブジェクトプールクラス
    /// </summary>
    /// <typeparam name="T">プールにするオブジェクトの型</typeparam>
    public class ObjectPool<T> where T : MonoBehaviour, IPoolObject
    {
        private T _origin;
        private Transform _poolParent;
        private List<T> _objcts = new List<T>();

        /// <summary>
        /// プールのセットアップ
        /// </summary>
        /// <param name="poolTarget">プールオブジェクト</param>
        /// <param name="count">最初の生成数</param>
        /// <param name="parent">プールをまとめるtransform</param>
        public void SetUp(T poolTarget, Transform parent, int count = 10)
        {
            _origin = poolTarget;
            _objcts = new List<T>();
            _poolParent = new GameObject(poolTarget.name + " Pool").transform;

            _poolParent.SetParent(parent);

            for (int i = 0; i < count; i++)
            {
                var newObj = Object.Instantiate(_origin, _poolParent);
                newObj.gameObject.SetActive(false);
                newObj.SetUp();
                _objcts.Add(newObj);
            }
        }
        /// <summary>
        /// 使われてないオブジェクトを取り出す
        /// </summary>
        /// <returns>使われてないオブジェクト</returns>
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
        /// <summary>
        /// プールを削除
        /// </summary>
        public void Clear()
        {
            foreach (var obj in _objcts)
            {
                Object.Destroy(obj.gameObject);
            }
            _objcts.Clear();
        }
    }
}
