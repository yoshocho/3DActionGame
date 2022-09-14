using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewFieldManager : MonoBehaviour
{
    [SerializeField]
    GameData _gameData;

    public FieldData FieldData { get; private set; } = new FieldData();

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
    int _fieldCount;
    bool _waveClear;

    [SerializeField]
    Vector3 _spawnCenter = Vector3.zero;
    [SerializeField]
    Vector3 _spawnLength = new Vector3(30.0f, 0.5f, 30.0f);

    private void Awake()
    {
        ServiceLocator<NewFieldManager>.Register(this);
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

        if(_waveClear && _gameData.WavesData.Count <= CurrentWave)
        {
            print("ÉQÅ[ÉÄÉNÉäÉA");
            GameManager.Instance.GameStateEvent(GameManager.GameState.GameClear);
            return;
        }

        if (_waitWave)
        {
            _waveWaitTimer += Time.deltaTime;
            print("waveë“Çøéûä‘");

            if (_waveWaitTimer > _waveWaitTime)
            {

                _waveSpawnEnd = false;
                _waitWave = false;
                _waveWaitTimer = 0.0f;
            }
        }

        if (!_waitWave && !_waveSpawnEnd)
        {
            _spawnTimer += Time.deltaTime;

        }

        if (_spawnTimer > _spawnTime)
        {

            var enemy = _gameData.WavesData[CurrentWave].Enemys[_spawanCount];
            Instantiate(enemy, GetRandomPos(), Quaternion.identity);
            _spawanCount++;
            print("ìGê∂ê¨");
            if (_spawanCount >= _gameData.WavesData[CurrentWave].Enemys.Count)
            {
                _waveClear = false;
                CurrentWave++;
                _fieldCount = _spawanCount;
                _spawanCount = 0;
                _waveSpawnEnd = true;
                print("waveê∂ê¨èIóπ");
            }
            _spawnTimer = 0.0f;
        }

        CheckWaveClear();
    }

    void CheckWaveClear()
    {
        if (_fieldCount <= _deathCount && _waveSpawnEnd)
        {
            _fieldCount = 0;
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