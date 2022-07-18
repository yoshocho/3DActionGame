using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance { get; private set; }
    [SerializeField]
    GameData _gameData;
    public GameData GameData => _gameData;

    [SerializeField]
    GameObject _enemyPrefab;

    [SerializeField]
    float _waitTime = 20.0f;
    List<Transform> _spawnPoints = new List<Transform>();
    float _timer;
    int _fieldCount = 0;
    int _currentWaveCount = 0;
    int _spawnCount = 0;
    bool _canSpawn = true;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        var points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < points.Length; i++)
        {
            _spawnPoints.Add(points[i].transform);
        }
        _timer = _waitTime;
        _fieldCount = GameData.WavesData[0].EnemyCount;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStart) return;

        UpdateEnemy();
    }

    public void DeathRequest(EnemyBase enemy)
    {
        GameManager.Instance.FieldData.RemoveEnemy(enemy);
        enemy.Death();
        _fieldCount--;
    }

    public void UpdateEnemy()
    {
        #region
        //if (_spawnCount >= _spawnPoints.Count) return;
        //if (_fieldCount >= GameData.WavesData[_currentWaveCount].EnemyCount)
        //{
        //    for (int i = 0; i < _spawnPoints.Count; i++)
        //    {
        //        var enemy = _enemyPool.Get();
        //        Spawn(enemy.gameObject, _spawnPoints[i].transform.position);
        //        if (_spawnCount == GameData.WavesData[_currentWaveCount].EnemyCount)
        //        {
        //            _spawnCount = 0;
        //            break;
        //        }
        //    }
        //    _currentWaveCount++;
        //    _fieldCount = GameData.WavesData[_currentWaveCount].EnemyCount;
        //    if (_currentWaveCount == GameData.WavesData.Count)
        //    {
        //        Debug.Log("終了");
        //    }
        //}
        #endregion

        _timer += Time.deltaTime;
        if(_timer > _waitTime) 
        {
            _timer = 0.0f;
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnCount == GameData.WavesData[_currentWaveCount].EnemyCount)
            {
                _spawnCount = 0;
                _currentWaveCount++;
                break;
            }
            
            Spawn(GameData.WavesData[_currentWaveCount].EnemyPrefab, _spawnPoints[i].transform.position);
           
        }
        if (_currentWaveCount == GameData.WavesData.Count)
        {
            Debug.Log("ゲームクリア");
        }
    }

    void Spawn(GameObject enemyObj, Vector3 point)
    {
        var obj = Instantiate(enemyObj,point,Quaternion.identity);
        _spawnCount++;
    }
}
