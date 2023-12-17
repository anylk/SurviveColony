using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private TextMeshProUGUI fpsText;
    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
        frameDeltaTimeArray = new float[50];
    }

    private void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;


        fpsText.text = "FPS\n" + GetColorfulText(Mathf.RoundToInt(CalculateFPS()));
    }
    private string GetColorfulText(int _fps)
    {
        if (_fps < 30)
        {
            return "<color=red>" + _fps.ToString() + "</color>";
        }
        else if (_fps < 50)
        {
            return "<color=yellow>" + _fps.ToString() + "</color>";
        }
        else
        {
            return "<color=green>" + _fps.ToString() + "</color>";
        }
    }

    private float CalculateFPS()
    {
        float total = 0;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }
}