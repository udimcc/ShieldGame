using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float damage = 10;
    public LayerMask shieldLayer;

    LayerMask hitLayer = new LayerMask();
    float defaultDestroyTime;
    bool isValid = false;
    float speed;

    // Private refs
    Rigidbody2D _rb;


    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody2D>();
    }

    public LayerMask HitLayer
    {
        get { return hitLayer; }
        set { hitLayer = value; }
    }

    public void Initialize(int projectileLayer, LayerMask hitLayer, float speed, float defaultDestroyTime=10f)
    {
        this.gameObject.layer = projectileLayer;
        this.hitLayer = hitLayer;
        this.speed = speed;
        this.defaultDestroyTime = defaultDestroyTime;
        this.isValid = true;
    }

    private void Start()
    {
        Destroy(this.gameObject, this.defaultDestroyTime);
    }

    private void Update()
    {
        if (this._rb.velocity.magnitude != this.speed)
        {
            this._rb.velocity = this._rb.velocity.normalized * this.speed;
        }
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
