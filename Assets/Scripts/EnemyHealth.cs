using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] Animator _Animator;
    public event Action OnDeath;
    public float _Health;
    public float maxHealth;
    [Header("Loot Settings")]
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 5;
    [SerializeField] private Transform lootspawn;
    [SerializeField] Image HB;
    public bool isDead;

    public void TakeDamage(float damage)
    {
        _Health -= damage;
        HB.fillAmount = _Health / maxHealth;
        if(_Health <= 0)
        {
            isDead = true;
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
