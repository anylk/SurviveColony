using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private InputType inputType { get { return GameManager.instance.inputType; } }

    public PlayerState state;
    public Vector3 rawInputMovement;

    private PlayerMovement movement;
    private WeaponController weaponController;
    public FloatingJoystick joystick;

    public float hitpoint = 100f;
    public float armor = 0f;

    private void Awake()
    {
        instance = this;
        movement = GetComponent<PlayerMovement>();
        weaponController = GetComponent<WeaponController>();
    }

    private void Start()
    {
        GameplayUI.instance.HitpointSlider(hitpoint);
        GameplayUI.instance.ArmorSlider(armor);
    }

    private void Update()
    {
        UpdateInput();
        Vector3 normalizeInputMovement = rawInputMovement.normalized;

        if (state == PlayerState.Move)
        {
            movement.UpdateInput(rawInputMovement);
        }
    }
    public void SetState(PlayerState state)
    {
        this.state = state;
        switch (state)
        {
            case PlayerState.Drive:
                break;
            case PlayerState.Move:
                break;
        }
    }

    private void UpdateInput()
    {
        float vertical;
        float horizontal;
        switch (inputType)
        {
            case InputType.Mobile:
                vertical = joystick.Vertical;
                horizontal = joystick.Horizontal;
                break;
            case InputType.Window:
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal");
                break;
            default:
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal");
                break;
        }
        rawInputMovement = new Vector3(horizontal, 0, vertical).normalized;
    }

    public float SetVisionRange(float newVisionRange)
    {
        return newVisionRange;
    }

    public void SetEnemy(Enemy e)
    {
        movement.SetIndicate(e);
    }
}
public enum PlayerState
{
    Move,
    Drive
}