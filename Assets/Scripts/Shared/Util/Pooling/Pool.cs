using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PoolPrefab
{
    public string name;
    public GameObject prefab;
}

public class Pool : MonoBehaviour
{
    public static Pool current;

    public Transform leftUpBound;
    public Transform rightDownBound;

    public PoolPrefab[] prefabsData;

    protected Dictionary<string, GameObject> prefabs;
    protected Dictionary<string, Queue<GameObject>> pools;

    /// ===========================================================================
    public static GameObject Instantiate(string prfbName, Vector3 position, Quaternion rotation)
    {
        if (!current.pools.ContainsKey(prfbName))
        {
            current.pools.Add(prfbName, new Queue<GameObject>());
        }

        var queue = current.pools[prfbName];

        GameObject result;

        if (queue.Count == 0)
        {
            if (!current.prefabs.ContainsKey(prfbName))
                throw new System.Exception($"Prefab {prfbName} is not available!");

            var prefab = current.prefabs[prfbName];
            result = GameObject.Instantiate(prefab, position, rotation);

            result.transform.parent = current.transform;
        }
        else
        {
            result = queue.Dequeue();
            result.transform.position = position;
            result.transform.rotation = rotation;

            result.SetActive(true);
        }

        return result;
    }

    /// ===========================================================================
    public static void Destroy(string prfbName, GameObject obj)
    {
        obj.SetActive(false);

        if (!current.pools.ContainsKey(prfbName))
        {
            current.pools.Add(prfbName, new Queue<GameObject>());
        }

        var queue = current.pools[prfbName];
        queue.Enqueue(obj);
    }

    /// ===========================================================================
    public static bool IsOutOfBounds(Vector3 position)
    {
        if (position.x < current.leftUpBound.position.x)
            return true;
        if (position.x > current.rightDownBound.position.x)
            return true;

        if (position.y > current.leftUpBound.position.y)
            return true;
        if (position.y < current.rightDownBound.position.y)
            return true;

        return false;
    }

    /// ===========================================================================
    private void Awake()
    {
        current = this;
        this.pools = new Dictionary<string, Queue<GameObject>>();
        this.prefabs = new Dictionary<string, GameObject>();

        // Prepare the prefabs and the pools
        foreach (var prefabData in this.prefabsData)
        {
            this.prefabs.Add(prefabData.name, prefabData.prefab);
            this.pools.Add(prefabData.name, new Queue<GameObject>());
        }
    }

    private void OnDrawGizmos()
    {
        if (this.leftUpBound != null && this.rightDownBound != null)
        {
            Gizmos.color = Color.white;
            Vector3 center = Vector3.Lerp(this.leftUpBound.position, this.rightDownBound.position, 0.5f);
            Vector3 size = this.leftUpBound.position - this.rightDownBound.position;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
