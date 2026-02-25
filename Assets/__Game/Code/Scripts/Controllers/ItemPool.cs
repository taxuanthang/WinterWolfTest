using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> pool
           = new Dictionary<GameObject, Queue<GameObject>>();

    public GameObject Get(GameObject prefab, Transform parent)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        if (pool[prefab].Count > 0)
        {
            GameObject obj = pool[prefab].Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(parent);
            return obj;
        }

        GameObject newObj = Instantiate(prefab, parent);
        return newObj;
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        pool[prefab].Enqueue(obj);
    }
}