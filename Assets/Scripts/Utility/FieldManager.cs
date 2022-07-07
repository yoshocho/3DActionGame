using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class FieldManager : MonoBehaviour
{
    public FieldManager Instance { get; private set; }
    [SerializeField]
    GameData _gameData;
    public GameData GameData => _gameData;

    [SerializeField]
    public int ClearCount = 20;
   
    [SerializeField]
    GameObject _enemyPrefab;

    List<Transform> _spawnPoints = new List<Transform>();

    ObjectPool<NormalStateEnemy> _enemyPool = new ObjectPool<NormalStateEnemy>();

    float _timer;
    int _fieldCount = 0;
    int _currentWaveCount = 0;
    int _spawnCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _enemyPool.SetUp(_enemyPrefab.GetComponent<NormalStateEnemy>(), transform, 10);
        var points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < points.Length; i++)
        {
            _spawnPoints.Add(points[i].transform);
        }

        _fieldCount = GameData.WavesData[0].EnemyCount;
    }

    //void Enemy

    private void Update()
    {
        if (!GameManager.Instance.GameStart) return;

        UpdateEnemy();
    }

    public void UpdateEnemy()
    {
        if (_spawnCount >= _spawnPoints.Count) return;

        if (_fieldCount >= GameData.WavesData[_currentWaveCount].EnemyCount)
        {
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                var enemy = _enemyPool.Get();
                Spawn(enemy.gameObject, _spawnPoints[i].transform.position);
                if (_spawnCount == GameData.WavesData[_currentWaveCount].EnemyCount) break;
            }
            _currentWaveCount++;
            _fieldCount = GameData.WavesData[_currentWaveCount].EnemyCount;
            if (_currentWaveCount == GameData.WavesData.Count)
            {
                Debug.Log("I—¹");
            }

        }
    }
    void Spawn(GameObject enemyObj, Vector3 point)
    {
        enemyObj.transform.position = point;
        _spawnCount++;
    }
}
