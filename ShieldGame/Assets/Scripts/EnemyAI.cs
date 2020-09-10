using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float discoverPlayerDist = 10f;
    public float fireRate = 1f;
    public float bulletForce = 100f;
    public LayerMask enemyLayer = new LayerMask();
    public Transform firePos;

    public GameObject projectile;

    GameObject player;

    float fireFrequency;
    float nextFiringTime = 0;

    void Awake()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.fireFrequency = 1 / this.fireRate;
    }

    void Update()
    {
        this.nextFiringTime -= Time.deltaTime;

        if (this.nextFiringTime <= 0)
        {
            this.nextFiringTime = this.fireFrequency;

            if (this.player == null)
            {
                return;
            }

            GameObject projectileObj = Instantiate(this.projectile, this.firePos.transform.position, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            Rigidbody2D projRb = projectileObj.GetComponent<Rigidbody2D>();

            if (projectile == null)
            {
                Debug.LogError("Projectile prefab doesn't have Projectile component");
                return;
            }

            if (projRb == null)
            {
                Debug.LogError("Projectile prefab doesn't have Rigidbody2D component");
                return;
            }

            projectile.Initialize(this.enemyLayer);

            Vector2 fireDir = (this.player.transform.position - this.firePos.transform.position).normalized;
            projRb.AddForce(fireDir * this.bulletForce, ForceMode2D.Impulse);
        }
    }
}
