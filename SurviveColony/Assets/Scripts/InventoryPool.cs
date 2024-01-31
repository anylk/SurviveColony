using UnityEngine;
using UnityEngine.Pool;

public class InventoryPool : MonoBehaviour
{
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    public InventoryWorldItem prefab;
    IObjectPool<InventoryWorldItem> m_Pool;

    public IObjectPool<InventoryWorldItem> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<InventoryWorldItem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }
            return m_Pool;
        }
    }

    InventoryWorldItem CreatePooledItem()
    {
        var go = Instantiate(prefab);
        return go;
    }

    void OnReturnedToPool(InventoryWorldItem system)
    {
        system.ResetObject();
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(InventoryWorldItem system)
    {
        system.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(InventoryWorldItem system)
    {
        Destroy(system.gameObject);
    }
}