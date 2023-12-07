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
        foreach (GameObject enemy in _enemyPool)        // Loop through the enemy container              
        {
            if (!enemy.activeInHierarchy && enemyPrefab.tag == enemy.tag)             // Check if active and correct name      
            {
                enemy.SetActive(true);                          
                return enemy;                                   
            }
        }

        // Dynamically create an Enemy if needed
        for (int i = 0; i < _enemyPrefabs.Length; i++)      // Loop through 5 times
        {
            if (enemyPrefab.tag == _enemyPrefabs[i].tag)        // Check that the paramater GameObject tag EQUAL to array tag
            {
                GameObject newEnemy = Instantiate(_enemyPrefabs[i], transform.position, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                _enemyPool.Add(newEnemy);
                Debug.Log("Built new enemy on the fly");
                return newEnemy;
            }
        }

        GameObject empty = null;
        Debug.Log("Returned empty GameObject in EnemyObjectPool - Trying to create enemy on the fly");
        return empty;
    }
}
