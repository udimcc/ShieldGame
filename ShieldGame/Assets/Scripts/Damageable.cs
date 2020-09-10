using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100f;

    [SerializeField] float currentHealth;
    List<UnityEvent> onHitList = new List<UnityEvent>();
    List<UnityEvent> onDestroyList = new List<UnityEvent>();
    bool defaultDestroy = true;

    void Start()
    {
        this.currentHealth = this.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        this.currentHealth -= damage;

        if (this.currentHealth <= 0)
        {
            foreach (UnityEvent onDestroy in this.onDestroyList)
            {
                onDestroy.Invoke();
            }

            if (this.defaultDestroy)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            foreach (UnityEvent onHit in this.onHitList)
            {
                onHit.Invoke();
            }
        }
    }

    public void GainHealth(float healthAmount)
    {
        this.currentHealth = Mathf.Min(this.maxHealth, this.currentHealth + healthAmount);
    }

    public void DisableDefaultDestroy()
    {
        this.defaultDestroy = false;
    }
}
