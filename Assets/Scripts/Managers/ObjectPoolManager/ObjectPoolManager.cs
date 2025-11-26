using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    private GameObject emptyParent;
    private static GameObject enemiesParent;
    private static GameObject projectilesParent;

    private static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    private static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType
    {
        Enemies,
        Projectiles
    }

    public static PoolType PoolingType;

    void Awake()
    {
        objectPools = new();
        cloneToPrefabMap = new();
        SetupEmptyGameObjectParents();
    }
    private void SetupEmptyGameObjectParents() // cria na hierarquia os gameObject parent para organizar
    {
        emptyParent = new GameObject("Object Pools");

        enemiesParent = new GameObject("Enemies");
        enemiesParent.transform.parent = emptyParent.transform;

        projectilesParent = new GameObject("Projectiles");
        projectilesParent.transform.parent = emptyParent.transform;

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
        objectPools.Add(prefab, newPool);
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
        if (cloneToPrefabMap.ContainsKey(obj))
        {
            cloneToPrefabMap.Remove(obj);
        }
    }
    private static GameObject SetParentObject(PoolType poolType)
    {
        return poolType switch
        {
            PoolType.Enemies => enemiesParent,
            PoolType.Projectiles => projectilesParent,
            _ => null,
        };
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 pos, Quaternion rotation, PoolType poolType = PoolType.Enemies) where T : Object
    {
        if (!objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, poolType);
        }
        GameObject obj = objectPools[objectToSpawn].Get();

        if (obj == null)
        {
            return null;
        }
        if (!cloneToPrefabMap.ContainsKey(obj))
        {
            cloneToPrefabMap.Add(obj, objectToSpawn);
        }

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
        if (cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("ObjectPoolManager: Trying to return an objec that isnt pooled: " + obj.name);

        }
    }

    public static void RegisterPreLoadedObject(GameObject preloadedObject, GameObject prefab, PoolType poolType = PoolType.Enemies)
    {
        if (!cloneToPrefabMap.ContainsKey(preloadedObject))
        {
            cloneToPrefabMap.Add(preloadedObject, prefab);
        }
        if (!objectPools.ContainsKey(prefab))
        {
            CreatePool(prefab, poolType);
        }
    }
}


