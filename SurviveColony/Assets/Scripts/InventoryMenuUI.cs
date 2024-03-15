using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuUI : MonoBehaviour
{
    [SerializeField] private InventoryItemUI inventoryItemUIPrefab;
    private List<InventoryItemUI> inventoryItemUI = new List<InventoryItemUI>();
    public GameObject menu;
    public Button closeButton;
    public Button openButton;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseIncentoryMenu);
        openButton.onClick.AddListener(OpenIncentoryMenu);
    }

    public void InitilizeInventoryMenu(List<InventoryItem> inventoryItems)
    {
        int itemCount = 0;
        foreach (InventoryItem item in inventoryItems)
        {
            if (inventoryItemUI.Count <= inventoryItems.Count)
            {
                InventoryItemUI newInventoryItemUI = Instantiate(inventoryItemUIPrefab, inventoryItemUIPrefab.transform.parent);
                inventoryItemUI.Add(newInventoryItemUI);
                newInventoryItemUI.SetData(item.data.displayName, item.data.icon, item.stackSize, item.data);
                newInventoryItemUI.Select(false);
                newInventoryItemUI.gameObject.SetActive(true);
                newInventoryItemUI.onSelected = SelectInventoryItem;
                itemCount++;
                continue;
            }
            inventoryItemUI[itemCount].SetData(item.data.displayName, item.data.icon, item.stackSize, item.data);
            inventoryItemUI[itemCount].Select(false);
            inventoryItemUI[itemCount].gameObject.SetActive(true);
            itemCount++;
        }

        if (inventoryItemUI.Count > itemCount)
        {
            for (int i = itemCount; i < inventoryItemUI.Count; i++)
            {
                inventoryItemUI[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectInventoryItem(InventoryItemUI inventoryItem)
    {
        foreach (var item in inventoryItemUI)
        {
            if (item == inventoryItem)
            {
                item.Select(true);
                continue;
            }
            item.Select(false);
        }
    }

    public void OpenIncentoryMenu()
    {
        openButton.interactable = false;
        menu.SetActive(true);
    }

    public void CloseIncentoryMenu()
    {
        openButton.interactable = true;
        menu.SetActive(false);
    }
}