using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{
    BulletPool pool;
    [Space(10), Header("Weapon Parts")]
    public Transform raycastOrigin;

    [Header("Weapon Stats")]
    public float accumulatedTime;
    public float fireRate = 1f;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0f;
    public float maxLifeTime = 3f;
    public float damage;

    [Space(10), Header("Sount Effects")]
    public AudioClip fireTriggerAudioClip;
    public AudioSource audioSource;

    [Space(10), Header("Bullet")]
    public TrailRenderer bulletPrefab;

    [Space(10), Header("Paricle Effects")]
    public ParticleSystem[] fireParticle;

    //Simulate Components
    private RaycastHit hitInfo;
    private List<TracerBullet> bullets = new List<TracerBullet>();
    private Ray ray;

    public override void Aim(int weight)
    {
        Debug.Log("Weight Setted = " + weight);
    }

    public override void Fire()
    {
        accumulatedTime = 0f;

        audioSource.PlayOneShot(fireTriggerAudioClip);

        foreach (ParticleSystem particle in fireParticle)
        {
            particle.Emit(1);
        }

        Vector3 velocity = raycastOrigin.forward * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    public override void Reload()
    {

    }

    public override void UpdateBullet(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    public override void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0.0f)
        {
            Fire();
            accumulatedTime -= fireInterval;
        }
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullets => bullets.time >= maxLifeTime);
    }

    public void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    public void RaycastSegment(Vector3 start, Vector3 end, TracerBullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            pool.EmitHitParicle(hitInfo);
            //hitEffectMetal.transform.position = hitInfo.point;
            //hitEffectMetal.transform.forward = hitInfo.normal;
            //hitEffectMetal.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;

            //IDamageable damageble = hitInfo.transform.GetComponent<IDamageable>();
            //if (damageble != null)
            //{
            //damageble.Damage(damage);
            //}
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    public TracerBullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        TracerBullet bullet = new TracerBullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(this.bulletPrefab, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }

    public Vector3 GetPosition(TracerBullet bullet)
    {
        //p+v*t+.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (raycastOrigin != null)
        {
            Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.forward * float.MaxValue);
        }
    }

    public override void Initiaze(BulletPool pool)
    {
        this.pool = pool;
    }
}
