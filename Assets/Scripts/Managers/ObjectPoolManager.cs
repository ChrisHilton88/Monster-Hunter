using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>   
{
    public int _bulletPoolCount { get; private set; } = 10;        // Sets the size of the bullet pool.
    public int _enemyPoolCount { get; private set; } = 5;           // Sets the size of the enemy pool.
    private int _maxEnemyPrefabPools { get; set; } = 5;            // Max size of each enemy type.

    private Vector3 _spawnPos = new Vector3(40, 0, 1);          // Spawn point for enemies.

    [SerializeField] private GameObject _bulletContainer;       // Container to store bullets.
    [SerializeField] private List<GameObject> _bulletPool;      // Size of bullet pool
    [SerializeField] private GameObject _bulletPrefab;          // Bullet prefab.

    [SerializeField] private GameObject _enemyContainer;        // Container to store our spawning enemies in the Hierarchy to keep it tidy.       
    [SerializeField] private List<GameObject> _enemyPool;       // Growing size of object pool as the waves increase.
    [SerializeField] private GameObject[] _enemyPrefabs;        // 5 prefab monsters to randomly select from or Level Designer can choose.

    //WaitForSeconds _instantiateTimer = new WaitForSeconds(0.25f);       // Spaces out the instantiation of the object pool (if needed - for optimisation).


    void OnEnable()
    {
        _enemyPool = GenerateEnemy(_enemyPoolCount);    
        _bulletPool = GenerateBullets(_bulletPoolCount);    
    }

    #region Bullet Object Pool
    // List of bullets genereated at the start of the game and set to INACTIVE.
    List<GameObject> GenerateBullets (int poolCount)
    {
        for (int i = 0; i < poolCount; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _spawnPos, Quaternion.identity);
            bullet.transform.parent = _bulletContainer.transform;           // Set new objects parent to the holding container.
            bullet.SetActive(false);            // Disable game object at start of game.
            _bulletPool.Add(bullet);            // Add bullet to the list
        }

        return _bulletPool;                     // Return a list of bullet GameObjects.
    }

    // Returns a Bullet GameObject, either from the pre-generated list or create one dynamically if needed.
    public GameObject RequestBullet(RaycastHit hitInfo)
    {
        foreach (GameObject bullet in _bulletPool)          // Loop through bullet pool
        {
            if(bullet.activeInHierarchy == false)           // Find first INACTIVE
            {
                bullet.SetActive(true);                     // Set to ACTIVE and assign position, rotation of passed in parameters.
                bullet.transform.position = hitInfo.point;
                //bullet.transform.rotation = rot;
                return bullet;                              // Return the object.
            }
        }

        // Dynamically create a bullet - This should not be needed!
        // If ALL Bullets are ACTIVE in Hierarchy, we have depleted our list - We must create one dynamically.
        GameObject newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);       // Instantiate new bullet.
        newBullet.transform.parent = _bulletContainer.transform;      // Set newBullet's parent to be the bullet Container.
        _bulletPool.Add(newBullet);                                   // Add the newBullet to the List 
        Debug.Log("Created Bullet on the fly!");
        return newBullet;                                            // Return this enemy GameObject instead
    }
    #endregion

    #region Enemy Object Pool
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

        // Dynamically create an Enemy - This should not be needed!
        // If ALL Enemies are ACTIVE in Hierarchy, we have depleted our list - We must create one dynamically.
        GameObject newEnemy = Instantiate(_enemyPrefabs[0], transform.position, Quaternion.identity);       // Instantiate newEnemy.
        newEnemy.transform.parent = _enemyContainer.transform;      // Set newEnemy's parent to be the Container.
        _enemyPool.Add(newEnemy);                                   // Add the newEnemy to the List
        Debug.Log("Created Enemy on the fly!");
        return newEnemy;                                            // Return this enemy GameObject instead
    }
    #endregion
}
