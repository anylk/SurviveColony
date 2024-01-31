using UnityEngine;
using UnityEngine.Pool;

public class IndicatorPool : MonoBehaviour
{
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    public IndicatorItemDisplay prefab;
    IObjectPool<IndicatorItemDisplay> m_Pool;

    public IObjectPool<IndicatorItemDisplay> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<IndicatorItemDisplay>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }
            return m_Pool;
        }
    }

    IndicatorItemDisplay CreatePooledItem()
    {
        var go = Instantiate(prefab);
        return go;
    }

    void OnReturnedToPool(IndicatorItemDisplay system)
    {
        system.SetData("");
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(IndicatorItemDisplay system)
    {
        system.gameObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(IndicatorItemDisplay system)
    {
        Destroy(system.gameObject);
    }
}