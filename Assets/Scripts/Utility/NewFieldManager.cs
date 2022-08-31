using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewFieldManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyGroupTest> _waveData = new List<EnemyGroupTest>();

    public FieldData FieldData { get; private set; } = new FieldData();

    public int CurrentWave { get; private set; } = 0;
    int _spawanCount = 0;
    [SerializeField]
    float _spawnTime = 0.5f;
    float _spawnTimer;

    [SerializeField]
    Vector3 _spawnCenter = Vector3.zero;
    [SerializeField]
    Vector3 _spawnLength = new Vector3(30.0f, 0.5f, 30.0f);

    private void Start()
    {
        

    }

    private void Update()
    {



        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > _spawnTime)
        {
            
            var enemy = _waveData[CurrentWave].Enemys[_spawanCount];
            Instantiate(enemy, GetRandomPos(), Quaternion.identity);
            _spawanCount++;

            if (_spawanCount >= _waveData[CurrentWave].Enemys.Count)
            {
                CurrentWave++;
                _spawanCount = 0;
            }

        }

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    var testObj =  GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    Instantiate(testObj, GetRandomPos(), Quaternion.identity);
        //}
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(-_spawnLength.x,_spawnLength.x) / 2;
        float z = Random.Range(-_spawnLength.z,_spawnLength.z) / 2;


        return new Vector3(x,0.5f,z) + _spawnCenter;
    }
    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_spawnCenter,_spawnLength);
    }
}
[System.Serializable]
public class EnemyGroupTest 
{
    public List<GameObject> Enemys = new List<GameObject>();

}

