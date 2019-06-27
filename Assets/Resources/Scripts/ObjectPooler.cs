using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;            // 생성 할 오브젝트 수
    public bool shouldExpand = true;    // 확장성

    public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
    {
        objectToPool = obj;
        amountToPool = Mathf.Max(amt, 2);
        shouldExpand = exp;
    }
}

[System.Serializable]
public class ObjectPooler : Singleton<ObjectPooler>
{
    public enum OBJECTPOOLER
    {
        Bullet = 0,
        ExplosionEffect,
        DefaultBalloon,
        Airship,
        PlayerDieBalloon,
    }

    public GameObject parent;

    public List<ObjectPoolItem> itemsToPool;

    public List<List<GameObject>> pooledObjectsList;
    public List<GameObject> pooledObjects;
    private List<int> position;

    public void Initialize()
    {
        pooledObjectsList = new List<List<GameObject>>();
        pooledObjects = new List<GameObject>();
        position = new List<int>();

        for (int i = 0; i < itemsToPool.Count; i++)
        {
            ObjectPoolItemToPooledObject(i);
        }
    }

    public GameObject GetPooledObject(int index)
    {
        int curSize = pooledObjectsList[index].Count;

        for (int i = position[index]; i < position[index] + pooledObjectsList[index].Count; i++)
        {
            if (!pooledObjectsList[index][i % curSize].activeInHierarchy)
            {
                position[index] = i % curSize;
                return pooledObjectsList[index][i % curSize];
            }
        }

        if (itemsToPool[index].shouldExpand)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(itemsToPool[index].objectToPool);
            obj.SetActive(false);
            obj.transform.SetParent(parent.transform);
            pooledObjectsList[index].Add(obj);
            return obj;
        }

        return null;
    }

    public List<GameObject> GetAllPooledObjects(int index)
    {
        return pooledObjectsList[index];
    }

    public int AddObject(GameObject obj, int amt = 3, bool exp = true)
    {
        ObjectPoolItem item = new ObjectPoolItem(obj, amt, exp);
        int currLen = itemsToPool.Count;
        itemsToPool.Add(item);
        ObjectPoolItemToPooledObject(currLen);
        return currLen;
    }

    private void ObjectPoolItemToPooledObject(int index)
    {
        ObjectPoolItem item = itemsToPool[index];

        pooledObjects = new List<GameObject>();
        for (int i = 0; i < item.amountToPool; i++)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(item.objectToPool);
            obj.SetActive(false);
            obj.transform.SetParent(parent.transform);
            pooledObjects.Add(obj);
        }
        pooledObjectsList.Add(pooledObjects);
        position.Add(0);
    }
}
