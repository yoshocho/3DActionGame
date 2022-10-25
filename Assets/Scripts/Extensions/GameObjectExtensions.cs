using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// �w�肳�ꂽ�C���^�[�t�F�C�X�����������R���|�[�l���g�����I�u�W�F�N�g���������ĕԂ�
    /// </summary>
    public static T FindObjectOfInterface<T>() where T : class
    {
        foreach (var n in GameObject.FindObjectsOfType<Component>())
        {
            var component = n as T;
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }
	/// <summary>
	/// �w�肳�ꂽ�C���^�[�t�F�C�X�����������R���|�[�l���g�����S�ẴI�u�W�F�N�g���������Ĕz���Ԃ�
	/// </summary>
	public static T[] FindObjectOfInterfaces<T>() where T : class
	{
		List<T> list = new List<T>();
		foreach (var n in GameObject.FindObjectsOfType<Component>())
		{
			var component = n as T;
			if (component != null)
			{
				list.Add(component);
			}
		}
		T[] ret = new T[list.Count];
		int count = 0;
		foreach (T component in list)
		{
			ret[count] = component;
			count++;
		}
		return ret;
	}
}
