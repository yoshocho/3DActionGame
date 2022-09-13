using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public List<GameObject> Enemys = new List<GameObject>();
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/EnemyGroupData")]
public class GameData : ScriptableObject
{
    [SerializeField]
    public List<WaveData> WavesData = new List<WaveData>();
}