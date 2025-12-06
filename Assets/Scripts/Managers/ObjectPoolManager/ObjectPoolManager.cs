using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private GameObject emptyParent;
    private static GameObject _enemiesParent;
    private static GameObject _projectilesParent;   

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        Enemies,
        Projectiles
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        SetupEmptyGameObjectParents();
    }
    private void SetupEmptyGameObjectParents() // cria na hierarquia os gameObject parent para organizar
    {
        emptyParent = new GameObject("Object Pools");

        _enemiesParent = new GameObject("Enemies")
        {
            transform =
            {
                parent = emptyParent.transform
            }
        };

        _projectilesParent = new GameObject("Projectiles")
        {
            transform =
            {
                parent = emptyParent.transform
            }
        };
    }

    // criar a pool em si
    private static void CreatePool(GameObject prefab, PoolType poolType = PoolType.Enemies)
    {
        // constructor da ObjectPool
        ObjectPool<GameObject> newPool = new(
            createFunc: () => CreateObject(prefab, poolType), // REVIEW - mexer nessa inicializacao com pos e rotation
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        _objectPools.Add(prefab, newPool);
    }

    // a pattern de objectPool consistem em 4 callbacks principais
    private static GameObject CreateObject(GameObject prefab, PoolType poolType = PoolType.Enemies)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, Vector3.one, Quaternion.identity);
        prefab.SetActive(true);
        GameObject parentObject = SetParentObject(poolType);
        obj.transform.parent = parentObject.transform;

        return obj;
    }

    private static void OnGetObject(GameObject obj)
    {
        // alguma logica
    }

    private static void OnReleaseObject(GameObject obj) // saiu da cena pra pool
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }
    private static GameObject SetParentObject(PoolType poolType)
    {
        return poolType switch
        {
            PoolType.Enemies => _enemiesParent,
            PoolType.Projectiles => _projectilesParent,
            _ => null,
        };
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 pos, Quaternion rotation, PoolType poolType = PoolType.Enemies) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, poolType);
        }
        GameObject obj = _objectPools[objectToSpawn].Get();

        if (!obj)
        {
            return null;
        }
        _cloneToPrefabMap.TryAdd(obj, objectToSpawn);

        obj.transform.SetPositionAndRotation(pos, rotation);
        obj.SetActive(true);

        if (typeof(T) == typeof(GameObject))
        {
            return obj as T;
        }


        if (!obj.TryGetComponent(out T component))
        {
            Debug.LogWarning("ObjectPoolManager: The spawned object does not have the requested component.");
        }
        return component;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 pos, Quaternion rotation, PoolType poolType = PoolType.Enemies) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, pos, rotation, poolType);
    }


    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 pos, Quaternion rotation, PoolType poolType = PoolType.Enemies)
    {
        return SpawnObject<GameObject>(objectToSpawn, pos, rotation, poolType);
    }


    public static void ReturnToPool(GameObject obj, PoolType poolType = PoolType.Enemies)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("ObjectPoolManager: Trying to return an object that isn't pooled: " + obj.name);

        }
    }

    public static void RegisterPreLoadedObject(GameObject preloadedObject, GameObject prefab, PoolType poolType = PoolType.Enemies)
    {
        _cloneToPrefabMap.TryAdd(preloadedObject, prefab);
        if (!_objectPools.ContainsKey(prefab))
        {
            CreatePool(prefab, poolType);
        }
    }
}


