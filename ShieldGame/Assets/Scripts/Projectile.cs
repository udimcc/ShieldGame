using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10;
    public LayerMask shieldLayer;

    LayerMask hitLayer = new LayerMask();
    float defaultDestroyTime;
    bool isValid = false;

    public LayerMask HitLayer
    {
        get { return hitLayer; }   // get method0
        set { hitLayer = value; }  // set method
    }

    public void Initialize(LayerMask hitLayer, float defaultDestroyTime=10f)
    {
        this.hitLayer = hitLayer;
        this.defaultDestroyTime = defaultDestroyTime;
        this.isValid = true;
    }

    private void Start()
    {
        Destroy(this.gameObject, this.defaultDestroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.isValid)
        {
            Debug.LogError("Projectile not initialized");
            return;
        }

        if (Utils.IsInLayerMask(this.shieldLayer, collision.collider.transform.gameObject.layer))
        {
            IShield shield = collision.collider.transform.GetComponent<IShield>();

            if (shield != null)
            {
                shield.OnProjectileCollision(this.gameObject);
            }
        }
        else
        {
            if (Utils.IsInLayerMask(this.hitLayer, collision.collider.transform.gameObject.layer))
            {
                Damageable damageable = collision.gameObject.transform.GetComponent<Damageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(this.damage);
                }
            }

            Destroy(this.gameObject);
        }
    }
}
