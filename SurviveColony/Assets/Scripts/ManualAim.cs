using UnityEngine;

public class ManualAim : MonoBehaviour
{
    public LayerMask aimLayerMask;

    private void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.instance.inputType == InputType.Window)
        {
            TrySelect(Input.mousePosition);
        }
    }

    void TrySelect(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, aimLayerMask))
        {
            transform.position = hit.point;
        }
    }
}
