using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    #region Singleton

    public static Layers inst;

    private void Awake()
    {
        Layers.inst = this;
    }

    #endregion

    public LayerMask blocking = new LayerMask();

    public static LayerMask Blocking { get { return Layers.inst.blocking; }}
    public static LayerMask Enemy { get { return LayerMask.NameToLayer("Enemy"); }}
    public static LayerMask EnemyProjectile { get { return LayerMask.NameToLayer("EnemyProjectile"); }}
    public static LayerMask PlayerProjectile { get { return LayerMask.NameToLayer("PlayerProjectile"); }}
}
