using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InventoryItemUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryItemName;
    public Image inventoryItemIcon;
    public TextMeshProUGUI inventoryItemAmount;
    public Image inventoryItemCover;
    public Button selectButton;
    public Button dropButton;

    private InventoryItemSO data;

    public Action<InventoryItemUI> onSelected;

    public GameObject inventoryItemInteraction;

    private void Start()
    {
        selectButton.onClick.AddListener(SelectButton);
        dropButton.onClick.AddListener(DropButton);
    }

    public void SetData(string name,Sprite icon,int amount, InventoryItemSO data)
    {
        this.data = data;
        inventoryItemName.text = name;
        inventoryItemAmount.text = "x" + amount.ToString();
        inventoryItemIcon.sprite = icon;
    }

    private void DropButton()
    {
        InventorySystem.instance.DropItem(data);
    }

    private void SelectButton()
    {
        onSelected?.Invoke(this);
    }

    public void Select(bool value)
    {
        inventoryItemCover.enabled = value;
        inventoryItemInteraction.gameObject.SetActive(value);
    }
}