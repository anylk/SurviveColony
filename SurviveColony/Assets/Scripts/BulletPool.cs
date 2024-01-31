using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public bool collectionChecks = true;
    public int maxPoolSize = 10;
    public Bullet prefab;
    IObjectPool<Bullet> m_Pool;

    [Header("Hit Particle")]
    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectMetal;
    public ParticleSystem hitEffectSand;
    public ParticleSystem hitEffectStone;
    public ParticleSystem hitEffectWood;
    public ParticleSystem hitEffectBlood;

    public IObjectPool<Bullet> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<Bullet>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }
            return m_Pool;
        }
    }

    Bullet CreatePooledItem()
    {
        var go = Instantiate(prefab);
        return go;
    }

    void OnReturnedToPool(Bullet system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(Bullet system)
    {
        system.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(Bullet system)
    {
        Destroy(system.gameObject);
    }

    public void EmitHitParicle(RaycastHit hitInfo)
    {
        int layerIndex = hitInfo.transform.gameObject.layer;
        //Wood 7 , stone 8, sand 9, metal 10, blood 11
        switch (layerIndex)
        {
            case 7:
                hitEffectWood.transform.position = hitInfo.point;
                hitEffectWood.transform.forward = hitInfo.normal;
                hitEffectWood.Emit(1);
                break;
            case 8:
                hitEffectStone.transform.position = hitInfo.point;
                hitEffectStone.transform.forward = hitInfo.normal;
                hitEffectStone.Emit(1);
                break;
            case 9:
                hitEffectSand.transform.position = hitInfo.point;
                hitEffectSand.transform.forward = hitInfo.normal;
                hitEffectSand.Emit(1);
                break;
            case 10:
                hitEffectMetal.transform.position = hitInfo.point;
                hitEffectMetal.transform.forward = hitInfo.normal;
                hitEffectMetal.Emit(1);
                break;
            case 11:
                hitEffectBlood.transform.position = hitInfo.point;
                hitEffectBlood.transform.forward = hitInfo.normal;
                hitEffectBlood.Emit(1);
                break;
            default:
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
                break;
        }
    }
}
