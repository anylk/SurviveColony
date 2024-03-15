using UnityEngine;

public class InventoryWorldItem : MonoBehaviour, ICollectableItem
{
    [SerializeField] private InventoryItemSO inventoryItemSO;
    [SerializeField] private int inventoryItemAmount;
    private MeshFilter itemMeshFilter;
    private Renderer itemRenderer;
    private MeshCollider itemMeshCollider;
    private Rigidbody rigidbody;

    public Vector3 worldPosition { get => transform.position; }
    public string displayName { get => "x" + inventoryItemAmount + "<#ffffff> " + inventoryItemSO.displayName; }
    public bool activeOnWorld { get => gameObject.activeInHierarchy; }

    public void ResetObject()
    {
        SetComponents();//only use on test
        itemMeshFilter.mesh = null;
        itemMeshCollider.sharedMesh = null;
        itemRenderer.material = null;
    }

    public void SetComponents()
    {
        itemMeshFilter = GetComponent<MeshFilter>();
        itemRenderer = GetComponent<Renderer>();
        itemMeshCollider = GetComponent<MeshCollider>();
    }

    public void InitializeData(InventoryItemSO inventoryItemSO,int inventoryItemAmount)
    {
        SetComponents();
        this.inventoryItemSO = inventoryItemSO;
        this.inventoryItemAmount = inventoryItemAmount;
        UpdateRenderer();
    }

    private void UpdateRenderer()
    {
        itemMeshFilter.mesh = inventoryItemSO.mesh;
        itemRenderer.material = inventoryItemSO.material;
        itemMeshCollider.sharedMesh = inventoryItemSO.mesh;
    }

    public void CollectItem()
    {
        InventorySystem.instance.Add(inventoryItemSO, this);
    }

    public void AddForce(Vector3 direction, float power)
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.AddForce(direction * power);
    }
    public void ToggleIndicate(bool setActive)
    {

    }
}
