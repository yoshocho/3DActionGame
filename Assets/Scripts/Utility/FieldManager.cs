using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    GameData _gameData;
    public int CurrentWave { get; private set; } = 0;
    int _spawanCount = 0;
    [SerializeField]
    float _spawnTime = 0.5f;
    float _spawnTimer;
    [SerializeField]
    float _waveWaitTime = 4.0f;
    float _waveWaitTimer;
    bool _waitWave;
    bool _waveSpawnEnd;
    int _deathCount = 0;
    int _targetCount;
    bool _waveClear;

    [SerializeField]
    Vector3 _spawnCenter = Vector3.zero;
    [SerializeField]
    Vector3 _spawnLength = new Vector3(30.0f, 0.5f, 30.0f);

    private void Awake()
    {
        ServiceLocator<FieldManager>.Register(this);
        //print(_gameData.WavesData.Count);
    }

    public void DeathRequest(EnemyBase enemy)
    {
        GameManager.Instance.FieldData.RemoveEnemy(enemy);
        _deathCount++;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.InGame) return;

        GameManager.Instance.UpdateGameTime();

        if(_waveClear && _gameData.WavesData.Count - 1 <= CurrentWave)
        {
            print("ゲームクリア");
            GameManager.Instance.GameStateEvent(GameManager.GameState.GameClear);
            return;
        }

        if (_waitWave)
        {
            _waveWaitTimer += Time.deltaTime;
            print("wave待ち時間");
            
            if (_waveWaitTimer > _waveWaitTime)
            {
                _waveClear = false;
                _waveSpawnEnd = false;
                _waitWave = false;
                _waveWaitTimer = 0.0f;
            }
        }

        CheckWaveClear();

        if (!_waitWave && !_waveSpawnEnd)
        {
            _spawnTimer += Time.deltaTime;

        }

        if (_spawnTimer > _spawnTime)
        {

            var enemy = _gameData.WavesData[CurrentWave].Enemys[_spawanCount];
            Instantiate(enemy, GetRandomPos(), Quaternion.identity);
            _spawanCount++;
            print("敵生成");
            if (_spawanCount >= _gameData.WavesData[CurrentWave].Enemys.Count)
            {
                _targetCount = _gameData.WavesData[CurrentWave].Enemys.Count;
                CurrentWave++;
                _spawanCount = 0;
                _waveSpawnEnd = true;
                print("wave生成終了");
            }
            _spawnTimer = 0.0f;
        }

        
    }

    void CheckWaveClear()
    {
        if (_targetCount <= _deathCount && _waveSpawnEnd && !_waitWave)
        {
            print("Waveクリア");
            _targetCount = 0;
            _deathCount = 0;
            _waveClear = true;
            _waitWave = true;
        }
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(-_spawnLength.x, _spawnLength.x) / 2;
        float z = Random.Range(-_spawnLength.z, _spawnLength.z) / 2;


        return new Vector3(x, 0.5f, z) + _spawnCenter;
    }
    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_spawnCenter, _spawnLength);
    }
}