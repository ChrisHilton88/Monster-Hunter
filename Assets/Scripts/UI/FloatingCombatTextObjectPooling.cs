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

    #region Request from Object Pools
    //Handles GameObject requests
    public GameObject RequestCriticalPrefab(Vector3 pos, Vector3 randomFactor)
    {
        foreach(GameObject prefab in _criticalTextPopUpList)
        {
            if(prefab.activeInHierarchy == false)
            {
                NewPosition(prefab, pos, randomFactor);  
                return prefab;
            }
        }

        Vector3 newObjectPos = pos + randomFactor;
        GameObject newObject = CreateSinglePrefab(_criticalTextPopUpPrefab, _criticalTextPopUpList, _criticalContainer, newObjectPos);
        return newObject;
    }

    public GameObject RequestNormalPrefab(Vector3 pos, Vector3 randomFactor)
    {
        foreach (GameObject prefab in _normalTextPopUpList)
        {
            if (prefab.activeInHierarchy == false)      
            {
                NewPosition(prefab, pos, randomFactor);
                return prefab;
            }
        }

        Vector3 newObjectPos = pos + randomFactor;
        GameObject newObject = CreateSinglePrefab(_normalTextPopUpPrefab, _normalTextPopUpList, _normalContainer, newObjectPos);
        return newObject;
    }

    // When an object is enabled, give it an updated position
    private void NewPosition(GameObject obj, Vector3 pos, Vector3 randomFactor)
    {
        obj.SetActive(true);     // Activate prefab from the List
        Vector3 newPos = pos + randomFactor;
        FloatingCombatTextAnimations temp = obj.GetComponent<FloatingCombatTextAnimations>();
        temp.UpdateOrigin(newPos);
    }

    // Create additional prefab on the fly if all the others are active
    GameObject CreateSinglePrefab(GameObject prefab, List<GameObject> list, GameObject container, Vector3 pos)
    {
        prefab = Instantiate(prefab, _spawnPos, Quaternion.identity);
        list.Add(prefab);
        prefab.transform.SetParent(container.transform);
        prefab.transform.position = pos;
        return prefab;
    }
    #endregion
}
