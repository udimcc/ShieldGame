using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushShield : MonoBehaviour, IShield
{
    public void OnProjectileCollision(GameObject projectileObj)
    {
        Destroy(projectileObj);
    }
}
