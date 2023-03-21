using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>   
{
    public int _poolCount { get; private set; }                 // Sets the size of the enemy pool.
    private int _maxEnemyPrefabPools { get; set; }              // Max size of each enemy type.

    private Vector3 _spawnPos = new Vector3(40, 0, 1);          // Spawn point for enemies.

    [SerializeField] private GameObject _enemyContainer;        // Container to store our spawning enemies in the Hierarchy to keep it tidy.       
    [SerializeField] private List<GameObject> _enemyPool;       // Growing size of object pool as the waves increase.
    [SerializeField] private GameObject[] _enemyPrefabs;        // 5 prefab monsters to randomly select from or Level Designer can choose.

    //WaitForSeconds _instantiateTimer = new WaitForSeconds(0.25f);       // Spaces out the instantiation of the object pool.


    void OnEnable()
    {
        _poolCount = 5;                                        // Total size of each enemy object pool.
        _maxEnemyPrefabPools = 5;
        _enemyPool = GenerateEnemy(_poolCount);                 
    }

    // Generates an Enemy List
    List<GameObject> GenerateEnemy (int poolCount)             // Return a List of GameObjects from the passed in EnemyCount value. 
    {
        for(int i = 0; i < poolCount; i++)                     // Loop through the EnemyCount value -> 0 to X.
        {
            for (int j = 0; j < _maxEnemyPrefabPools; j++)
            {
                GameObject enemy = Instantiate(_enemyPrefabs[j], _spawnPos, Quaternion.LookRotation(Vector3.left, Vector3.up));  // Instantiate a new enemy 
                enemy.transform.parent = _enemyContainer.transform;         // Set the new objects parent to be the Container.
                enemy.SetActive(false);                             // Set them all as false to begin the game with.
                _enemyPool.Add(enemy);                              // Make sure to add them to the list.
            }

        } 

        return _enemyPool;                                      // Return the enemy GameObject List.
    }

    //IEnumerator GenerateEnemyRoutine()
    //{

    //}


    // Request an Enemy from the List - To be called from the SpawnManager
    public GameObject RequestEnemy()                            // Return a GameObject - Either from the List or Create one on the fly if none available.
    {
        foreach(GameObject enemy in _enemyPool)                 // Loop through enemies.
        {
            if(enemy.activeInHierarchy == false)                // Find the first INACTIVE in Hierarchy.
            {
                enemy.SetActive(true);                          // Set to True.
                return enemy;                                   // Return this enemy GameObject.
            }
        }

        // Dynamically create an Enemy
        // If ALL Enemies are ACTIVE in Hierarchy, we have depleted our list - We must create one dynamically.
        GameObject newEnemy = Instantiate(_enemyPrefabs[0], transform.position, Quaternion.identity);       // Instantiate newEnemy.
        newEnemy.transform.parent = _enemyContainer.transform;      // Set newEnemy's parent to be the Container.
        _enemyPool.Add(newEnemy);                                   // Add the newEnemy to the List - Increasing it's size. Couldn't do this if an Array.
        Debug.Log("Create Enemy on the fly!");

        return newEnemy;                                            // Return this enemy GameObject instead
    }
}
