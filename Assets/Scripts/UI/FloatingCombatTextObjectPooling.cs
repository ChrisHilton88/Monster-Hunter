using System.Collections.Generic;
using UnityEngine;

public class FloatingCombatTextObjectPooling : MonoSingleton<FloatingCombatTextObjectPooling>
{
    private int _maxCriticalPrefabs = 20;
    private int _maxNormalPrefabs = 30;

    [SerializeField] private List<GameObject> _criticalTextPopUpList = new List<GameObject>();
    [SerializeField] private List<GameObject> _normalTextPopUpList = new List<GameObject>();

    [SerializeField] private GameObject _criticalTextPopUpPrefab;
    [SerializeField] private GameObject _normalTextPopUpPrefab;
    [SerializeField] private GameObject _criticalContainer;
    [SerializeField] private GameObject _normalContainer;

    Vector3 _spawnPos = Vector3.zero;


    void Start()
    {
        _criticalContainer = GameObject.Find("Critical Container");
        _normalContainer = GameObject.Find("Normal Container");
        CreateCriticalTexPopUpPool();
        CreateNormalTexPopUpPool();
    }

    #region Object Pools
    // Creates and returns the Critical list of prefabs
    List<GameObject> CreateCriticalTexPopUpPool()
    {
        CreatePool(_criticalTextPopUpPrefab, _maxCriticalPrefabs, _criticalTextPopUpList, _criticalContainer);
        return _criticalTextPopUpList;  
    }

    // Creates and returns the Normal list of prefabs
    List<GameObject> CreateNormalTexPopUpPool()
    {
        CreatePool(_normalTextPopUpPrefab, _maxNormalPrefabs, _normalTextPopUpList, _normalContainer);
        return _normalTextPopUpList;
    }

    void CreatePool(GameObject prefab, int poolSize, List<GameObject> list, GameObject container)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(prefab, _spawnPos, Quaternion.identity);
            list.Add(newObject);
            newObject.transform.SetParent(container.transform);
            newObject.SetActive(false);
        }
    }
    #endregion



    #region Request & Return from Object Pools
    //Handles GameObject requests
    public GameObject RequestCriticalPrefab()
    {
        foreach(GameObject prefab in _criticalTextPopUpList)
        {
            if(prefab.activeInHierarchy == false)
            {
                prefab.SetActive(true);
                return prefab;
            }

            Debug.Log("Test 1");
        }

        Debug.Log("Test 2");

        GameObject newObject = CreateSinglePrefab(_criticalTextPopUpPrefab, _criticalTextPopUpList, _criticalContainer);
        Debug.Log("Test 3");
        return newObject;
    }

    public GameObject RequestNormalPrefab()
    {
        GameObject newObject = null;

        foreach (GameObject prefab in _normalTextPopUpList)
        {
            if (prefab.activeInHierarchy == false)
            {
                prefab.SetActive(true);
                return prefab;
            }
        }

        GameObject temp = CreateSinglePrefab(_normalTextPopUpPrefab, _normalTextPopUpList, _normalContainer);
        newObject = temp;
        return newObject;
    }

    GameObject CreateSinglePrefab(GameObject prefab, List<GameObject> list, GameObject container)
    {
        prefab = Instantiate(prefab, _spawnPos, Quaternion.identity);
        list.Add(prefab);
        prefab.transform.SetParent(container.transform);
        return prefab;
    }
    #endregion
}
