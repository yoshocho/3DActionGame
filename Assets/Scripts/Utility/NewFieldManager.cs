using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFieldManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyGroupTest> _enemys = new List<EnemyGroupTest>();

    public FieldData FieldData { get; private set; } = new FieldData();

    List<Transform> _spawnPoints = new List<Transform>();

    [SerializeField]
    float _spawnTime;
    float _spawnTimer;

    private void Start()
    {
        var points = GameObject.FindGameObjectsWithTag("SpawnPoint");

        foreach (var point in points)
        {
            _spawnPoints.Add(point.transform);
        }

    }

    private void Update()
    {

    }
}
[System.Serializable]
public class EnemyGroupTest 
{
    public List<GameObject> Enemys = new List<GameObject>();

}

