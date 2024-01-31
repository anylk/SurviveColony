using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    public event Action onInventoryChangeEvent;

    private InventoryPool inventoryPool;

    private Dictionary<InventoryItemSO, InventoryItem> m_itemDictionaty;
    [field: SerializeField] public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventoryPool = GetComponent<InventoryPool>();
        inventory = new List<InventoryItem>();
        m_itemDictionaty = new Dictionary<InventoryItemSO, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemSO referenceData)
    {
        if (m_itemDictionaty.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemSO referanceData, InventoryWorldItem dropItem)
    {
        if (m_itemDictionaty.TryGetValue(referanceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referanceData);
            inventory.Add(newItem);
            m_itemDictionaty.Add(referanceData, newItem);
        }

        inventoryPool.Pool.Release(dropItem);

        if (onInventoryChangeEvent != null)
        {
            onInventoryChangeEvent();
        }
    }
    public void Remove(InventoryItemSO referenceData)
    {
        if (m_itemDictionaty.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionaty.Remove(referenceData);
            }
        }
        if (onInventoryChangeEvent != null)
        {
            onInventoryChangeEvent();
        }
    }

    [ContextMenu("Drop First Item")]
    public void DropFirstItem()
    {
        if (inventory.Count > 0)
        {
            InventoryItemSO inventoryItemSO = inventory[0].data;
            Remove(inventoryItemSO);
            InventoryWorldItem item = Instantiate(inventoryPool.Pool.Get(), transform.position, Quaternion.identity);
            item.transform.position = Vector3.one;
            item.InitializeData(inventoryItemSO,1);
        }
    }
}