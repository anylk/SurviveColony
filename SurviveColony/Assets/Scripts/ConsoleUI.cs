using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleUI : MonoBehaviour
{
    public static ConsoleUI instance;

    [SerializeField] private int consoleLines = 10;
    [SerializeField] private TextMeshProUGUI consoleText;
    private Queue<string> consoleQueue = new Queue<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddMessage(string message, Color color)
    {
        if (consoleQueue.Count >= consoleLines)
        {
            consoleQueue.Dequeue();
        }
        consoleQueue.Enqueue($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");

        UpdateConsoleText();
    }

    private void UpdateConsoleText()
    {
        string consoleTextContent = "";
        foreach (string line in consoleQueue)
        {
            consoleTextContent += $"{line}\n";
        }

        consoleText.text = consoleTextContent;
    }
}