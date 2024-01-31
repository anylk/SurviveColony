using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [field: SerializeField] public InventoryItemSO data { get; private set; }
    [field: SerializeField] public int stackSize { get; private set; }

    public InventoryItem(InventoryItemSO source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}