using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShield : MonoBehaviour, IShield
{
    public LayerMask enemyLayer = new LayerMask();


    public void OnProjectileCollision(GameObject projectileObj)
    {
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        if (projectile == null)
        {
            Debug.LogError("Projectile prefab doesn't have Projectile component");
            return;
        }

        projectile.HitLayer = enemyLayer;
    }
}
