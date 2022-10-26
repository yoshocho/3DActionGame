using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool
{
    /// <summary>
    /// �I�u�W�F�N�g�v�[���N���X
    /// </summary>
    /// <typeparam name="T">�v�[���ɂ���I�u�W�F�N�g�̌^</typeparam>
    public class ObjectPool<T> where T : MonoBehaviour, IPoolObject
    {
        private T _origin;
        private Transform _poolParent;
        private List<T> _objcts = new List<T>();

        /// <summary>
        /// �v�[���̃Z�b�g�A�b�v
        /// </summary>
        /// <param name="poolTarget">�v�[���I�u�W�F�N�g</param>
        /// <param name="count">�ŏ��̐�����</param>
        /// <param name="parent">�v�[�����܂Ƃ߂�transform</param>
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
        /// �g���ĂȂ��I�u�W�F�N�g�����o��
        /// </summary>
        /// <returns>�g���ĂȂ��I�u�W�F�N�g</returns>
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
        /// �v�[�����폜
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
