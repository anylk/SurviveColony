using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI instance;

    [Header("Player Stats")]
    [SerializeField] private RectTransform playerStatsRect;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider hitpointSlider;
    [SerializeField] private TextMeshProUGUI hitpointText;
    [SerializeField] private Slider armorSlider;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private Slider carHitpointSlider;

    [Header("Aim")]
    [SerializeField] private RectTransform aimRect;

    [Header("Inventory Menu")]
    [SerializeField] private InventoryMenuUI inventoryMenu;


    private void Awake()
    {
        instance = this;
    }

    public void StaminaSlider(float value)
    {
        staminaSlider.value = value;
    }

    public void HitpointSlider(float value)
    {
        hitpointSlider.value = value;
        hitpointText.text = value.ToString("F0");
    }

    public void ArmorSlider(float value)
    {
        armorSlider.value = value;
        armorText.text = value.ToString("F0");
    }

    public void CarHitpointSlider(float value)
    {
        carHitpointSlider.value = value;
    }

    public void EnableAim(bool isActive)
    {
        DOTween.Kill(aimRect.transform);
        if (isActive)
        {
            aimRect.gameObject.SetActive(isActive);
            aimRect.DOScale(Vector3.one, .25f);
        }
        else
        {
            
            aimRect.DOScale(Vector3.zero, .25f).OnComplete(() =>
            {
                aimRect.gameObject.SetActive(isActive);
            });
        }
    }

    public void SetAimScreenTransform(Vector2 screenPosition,Quaternion rotation)
    {
        aimRect.position = screenPosition;
        aimRect.rotation = rotation;
    }

    public void OpenInventoryMenu(List<InventoryItem> items)
    {
        inventoryMenu.InitilizeInventoryMenu(items);
    }
}