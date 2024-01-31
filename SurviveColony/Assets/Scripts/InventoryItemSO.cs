using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item")]
public class InventoryItemSO : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;

    [Space(20),Header("Renderer Stats")]
    public Mesh mesh;
    public Material material;
}