using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoSingleton<EnemyObjectPool>
{
    int _enemyPoolCount = 5;           
    int _maxEnemyPrefabPools = 5;            

    Vector3 _spawnPos = new Vector3(40, 0, 1);          

    [SerializeField] private GameObject _enemyContainer;           
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private List<GameObject> _enemyPool;


    void Start()
    {
        //_enemyPool = GenerateEnemy(_enemyPoolCount);
    }

    // Generates an Enemy List
    //List<GameObject> GenerateEnemy(int poolCount)             
    //{
    //    for (int i = 0; i < poolCount; i++)                     
    //    {
    //        for (int j = 0; j < _maxEnemyPrefabPools; j++)
    //        {
    //            GameObject enemy = Instantiate(_enemyPrefabs[j], _spawnPos, Quaternion.LookRotation(Vector3.left, Vector3.up));  
    //            enemy.transform.parent = _enemyContainer.transform;         
    //            enemy.SetActive(false);                             
    //            _enemyPool.Add(enemy);                              
    //        }
    //    }

    //    return _enemyPool;                                      
    //}

    // Request Enemy from the List
    public GameObject RequestEnemy()                            
    {
        foreach (GameObject enemy in _enemyPool)                 
        {
            if (enemy.activeInHierarchy == false)                
            {
                enemy.SetActive(true);                          
                return enemy;                                   
            }
        }

        // Dynamically create an Enemy if needed
        GameObject newEnemy = Instantiate(_enemyPrefabs[0], transform.position, Quaternion.identity);       
        newEnemy.transform.parent = _enemyContainer.transform;      
        _enemyPool.Add(newEnemy);                                   
        return newEnemy;                                            
    }
}
