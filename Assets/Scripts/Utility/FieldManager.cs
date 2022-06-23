using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPool;

public class FieldManager : MonoBehaviour
{
    public FieldManager Instance { get; private set; }

    [SerializeField]
    GameObject _enemyPrefab;
    
    ObjectPool<NormalStateEnemy> _enemys = new ObjectPool<NormalStateEnemy>();

    private void Awake()
    {
        Instance = this;

        _enemys.SetUp(_enemyPrefab.GetComponent<NormalStateEnemy>(), transform);
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SpawnEnemy(GameObject enemyObj,Vector3 point) 
    {
        enemyObj.transform.position = point;
    }
}
