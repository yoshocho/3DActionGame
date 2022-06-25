using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class FieldManager : MonoBehaviour
{
    public FieldManager Instance { get; private set; }

    [SerializeField]
    GameObject _enemyPrefab;

    [SerializeField]
    GameData _gameData;
    public GameData GameData => _gameData;

    int _waveCount = 0;
    List<Transform> _spawnPoints = new List<Transform>();
    List<CharacterBase> _fieldEnemy = new List<CharacterBase>();
    [SerializeField]
    int _fieldCount = 0;
    WaveData _currentWave;
    ObjectPool<NormalStateEnemy> _enemys = new ObjectPool<NormalStateEnemy>();

    private void Awake()
    {
        Instance = this;
        _enemys.SetUp(_enemyPrefab.GetComponent<NormalStateEnemy>(), transform,10);
    }
    private void Start()
    {
        var points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < points.Length; i++)
        {
            _spawnPoints.Add(points[i].transform);
        }
        //_currentWave = GameData.WaveDatas[0];
        _fieldCount = GameData.WaveDatas[0].WaveCount;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStart) return;
        
        if(_fieldCount == GameData.WaveDatas[_waveCount].WaveCount)
        {
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                Debug.Log("Sp");
                var enemy = _enemys.Get();
                Spawn(enemy.gameObject, _spawnPoints[i].transform.position);
            }
            _fieldCount = 0;
            _waveCount++;
        }
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    _fieldCount++;
        //}
    }
    public void SpawnWave() 
    {

    }
    void Spawn(GameObject enemyObj,Vector3 point) 
    {
        enemyObj.transform.position = point;
    }
}
