using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoSingleton<EnemyObjectPool>
{
    int _enemyPoolCount = 5;           
    int _maxEnemyPrefabCount = 5;

    [SerializeField] Transform _initialSpawnPos;
    [SerializeField] private GameObject _enemyContainer;           
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private List<GameObject> _enemyPool;


    void Start()
    {
        _enemyPool = GenerateEnemy(_enemyPoolCount, _maxEnemyPrefabCount);
    }

    // Generates an Enemy List
    // In the Inspector, this generates all 5 prefabs as a loop, 5 times. (instead of 5 ghouls, 5 cannibal etc)
    List<GameObject> GenerateEnemy(int poolCount, int enemiesPerPool)
    {
        for (int i = 0; i < poolCount; i++)
        {
            for (int j = 0; j < enemiesPerPool; j++)
            {
                GameObject enemy = Instantiate(_enemyPrefabs[j], _initialSpawnPos.position, Quaternion.LookRotation(Vector3.left, Vector3.up));
                enemy.transform.parent = _enemyContainer.transform;
                enemy.SetActive(false);
                _enemyPool.Add(enemy);
            }
        }

        return _enemyPool;
    }

    // Request Enemy from the List
    // TODO: Need to look for a specific enemy
    public GameObject RequestEnemy(GameObject enemyPrefab)                            
    {
        // Grab enemy if one is inactive in list
        foreach (GameObject enemy in _enemyPool)                      
        {
            if (!enemy.activeInHierarchy && enemyPrefab.tag == enemy.tag)                  
            {
                enemy.SetActive(true);                          
                return enemy;                                   
            }
        }

        // Else, dynamically create an Enemy if needed
        for (int i = 0; i < _enemyPrefabs.Length; i++)      
        {
            if (enemyPrefab.tag == _enemyPrefabs[i].tag)        
            {
                GameObject newEnemy = Instantiate(_enemyPrefabs[i], _initialSpawnPos.position, Quaternion.LookRotation(Vector3.left, Vector3.up));
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyPool.Add(newEnemy);
                return newEnemy;
            }
        }

        GameObject empty = null;
        Debug.Log("Returned empty GameObject in EnemyObjectPool - Trying to create enemy on the fly");
        return empty;
    }
}
