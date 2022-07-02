using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public GameObject EnemyPrefab;
    public int EnemyCount = 3;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField]
    public int ClearCount = 20;
    [SerializeField]
    public List<WaveData> WaveDatas = new List<WaveData>();
}
