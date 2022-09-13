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

    int _deathCount = 0;
    int _fieldCount;

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

        if(!_waitWave && _gameData.WavesData.Count <= CurrentWave)
        {
            GameManager.Instance.GameStateEvent(GameManager.GameState.GameClear);
            return;
        }

        if (_waveWaitTimer > 0.0f && _waitWave)
        {
            _waveWaitTimer -= Time.deltaTime;
            print("wave待ち時間");

            if (_waveWaitTimer < 0.0f)
            {
                _waveWaitTimer = 0.0f;
            }
        }

        if (_waveWaitTimer <= 0.0f && !_waitWave)
        {
            _spawnTimer += Time.deltaTime;

        }

        CheckWaveClear();

        if (_spawnTimer > _spawnTime)
        {

            var enemy = _gameData.WavesData[CurrentWave].Enemys[_spawanCount];
            Instantiate(enemy, GetRandomPos(), Quaternion.identity);
            _spawanCount++;
            print("敵生成");
            if (_spawanCount >= _gameData.WavesData[CurrentWave].Enemys.Count)
            {
                CurrentWave++;
                _fieldCount = _spawanCount;
                _spawanCount = 0;
                _waveWaitTimer = _waveWaitTime;
                _waitWave = true;

                print("wave生成終了");
            }
            _spawnTimer = 0.0f;
        }



        if (Input.GetKeyDown(KeyCode.X))
        {
            //var testObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //Instantiate(testObj, GetRandomPos(), Quaternion.identity);
            _waitWave = false;
        }
    }

    bool CheckWaveClear()
    {
        if (_fieldCount <= _deathCount) 
        {
            _fieldCount = 0;
            _deathCount = 0;
            _waitWave = false;
            return true;
        }
        else return false;
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