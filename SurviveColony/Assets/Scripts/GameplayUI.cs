using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
}