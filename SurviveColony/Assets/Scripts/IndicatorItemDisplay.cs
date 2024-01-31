using UnityEngine;
using TMPro;
public class IndicatorItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    public void SetData(string text)
    {
        displayText.text = text;
    }
}
