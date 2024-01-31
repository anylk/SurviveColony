using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public IKWeaponData ikWeaponData;

    public abstract void Initiaze(BulletPool pool);
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Aim(int weight);
    public abstract void UpdateFiring(float deltaTime);
    public abstract void UpdateBullet(float deltaTime);

}

[System.Serializable]
public class TracerBullet
{
    [HideInInspector] public float time;
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Vector3 initialVelocity;
    public TrailRenderer tracer;

    public void FireTrigger(Vector3 initPos, Vector3 initVelo)
    {
        tracer.transform.position = initPos;
        initialPosition = initPos;
        initialVelocity = initVelo;
        time = 0;
        tracer.AddPosition(initPos);
    }
}