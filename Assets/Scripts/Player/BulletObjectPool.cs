using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoSingleton<BulletObjectPool>   
{
    int _bulletPoolCount = 50;        

    Vector3 _spawnPos = new Vector3(40, 0, 1);         

    [SerializeField] private GameObject _bulletContainer;    
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> _bulletPool;      

    //WaitForSeconds _instantiateTimer = new WaitForSeconds(0.25f);       


    void Start()
    {
        _bulletPool = GenerateBullets(_bulletPoolCount);    
    }

    // Generate List of Bullets
    List<GameObject> GenerateBullets (int poolCount)
    {
        for (int i = 0; i < poolCount; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _spawnPos, Quaternion.identity);
            bullet.transform.parent = _bulletContainer.transform;           
            bullet.SetActive(false);            
            _bulletPool.Add(bullet);           
        }

        return _bulletPool;                     
    }

    // Request a Bullet Prefab
    public GameObject RequestBullet(RaycastHit hitInfo)
    {
        foreach (GameObject bullet in _bulletPool)          
        {
            if(bullet.activeInHierarchy == false)           
            {
                bullet.SetActive(true);                     
                bullet.transform.position = hitInfo.point;
                return bullet;                              
            }
        }

        // Dynamically create a bullet if needed
        GameObject newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);       
        newBullet.transform.parent = _bulletContainer.transform;      
        _bulletPool.Add(newBullet);                                   
        return newBullet;                                            
    }
}
