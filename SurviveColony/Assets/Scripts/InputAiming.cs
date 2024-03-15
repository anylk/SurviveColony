using UnityEngine;
using UnityEngine.UI;

public class InputAiming : MonoBehaviour
{
    public LayerMask aimLayerMask;
    public Vector3 rayOffset;
    private Vector3 aimPosition;

    public KeyCode focusKeyCode;
    public KeyCode shootKeyCode;

    public bool inputFocus;
    public bool inputShoot;
    public Transform aimDot;
    public WeaponController weaponController;
    public PlayerMovement playerMovement;

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }


    void Update()
    {
        if (Input.GetKeyDown(focusKeyCode))
        {
            inputFocus = true;
            CheckAim();
            GameManager.instance.SetCursorVisiblity(!inputFocus);
        }
        if (Input.GetKeyDown(shootKeyCode))
        {
            if (inputFocus)
            {
                inputShoot = inputFocus;
                CheckAim();
            }
        }
        if (Input.GetKeyUp(focusKeyCode))
        {
            inputFocus = false;
            CheckAim();
            GameManager.instance.SetCursorVisiblity(!inputFocus);
        }
        if (Input.GetKeyUp(shootKeyCode))
        {
            inputShoot = false;
            CheckAim();
        }

        if (GameManager.instance.inputType == InputType.Window)
        {
            TrySelect(Input.mousePosition);

            transform.position = aimPosition;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(aimPosition);

            float normalizedX = screenPos.x / screenWidth;
            float normalizedY = screenPos.y / screenHeight;
            float angle = Mathf.Atan2(normalizedY - 0.5f, normalizedX - 0.5f) * Mathf.Rad2Deg;
            angle -= 90f;
            Quaternion screenRotation = Quaternion.Euler(0, 0, angle);

            GameplayUI.instance.SetAimScreenTransform(screenPos, screenRotation);
        }
    }

    void TrySelect(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, aimLayerMask))
        {
            if (inputFocus)
            {
                aimPosition = hit.collider.ClosestPoint(hit.point);
            }
        }
    }
    public void CheckAim()
    {
        playerMovement.AimIndicate(inputFocus);
        weaponController.aiming = inputFocus;
        weaponController.autoFire = inputShoot;
        CameraSystem.instance.EnableAnimCamera(inputFocus);
        GameplayUI.instance.EnableAim(inputFocus);
    }
}
