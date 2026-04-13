using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public event Action OnDeath;

    public float _Health;

    public void TakeDamage(float damage)
    {
        _Health -= damage;
        if(_Health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("1");
        OnDeath?.Invoke();
        Debug.Log("2");
        gameObject.SetActive(false);
    }
}
