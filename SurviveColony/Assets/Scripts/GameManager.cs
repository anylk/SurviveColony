using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public InputType inputType;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void SetCursorVisiblity(bool isVisible)
    {
        Cursor.visible = isVisible;
    }
}
public enum InputType
{
    Mobile = 2,
    Window = 4,
}