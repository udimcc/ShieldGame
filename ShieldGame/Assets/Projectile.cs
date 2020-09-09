using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10;

    LayerMask hitLayer = new LayerMask();
    float defaultDestroyTime;
    bool isValid = false;

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

        if ((this.hitLayer.value & (1 << collision.gameObject.layer)) != 0)
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
