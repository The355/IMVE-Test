using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class q1_pool : MonoBehaviour
{
    List<GameObject> pool, inactivePool;
    [SerializeField] GameObject baseObj; // This is prefab reference

    void Awake()
    {
        pool = new List<GameObject>();
    }

    void ResetObjectState(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;

        // Do other stuff, ex. reset anim, reset velocity, etc etc
    }

    void SpawnObject()
    {
        if (inactivePool.Count == 0)
        {
            var newObj = Instantiate(baseObj, Vector3.zero, Quaternion.identity);
            pool.Add(newObj);
        }
        else
        {
            var oldObj = inactivePool[0];
            inactivePool.RemoveAt(0);

            ResetObjectState(oldObj);
            oldObj.SetActive(true);
        }

    }

    void DespawnObject(GameObject obj)
    {
        inactivePool.Add(obj);
        obj.SetActive(false);
    }
}
