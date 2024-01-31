using UnityEngine;

public interface IIndicatable
{
    public void ToggleIndicate(bool setActive);
    public Vector3 worldPosition { get; }
    public string displayName { get; }
}