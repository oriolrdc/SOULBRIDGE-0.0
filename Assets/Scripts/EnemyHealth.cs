using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    public float _Health;
    [Header("Loot Settings")]
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 5;
    [SerializeField] private Transform lootspawn;

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
        int amount = UnityEngine.Random.Range(minCoins, maxCoins + 1);

        for (int i = 0; i < amount; i++)
        {
            GameObject Coin = PoolManager.Instance.GetPooledObject("Coins", lootspawn.position, lootspawn.rotation);
            Coin.SetActive(true);
        }
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
