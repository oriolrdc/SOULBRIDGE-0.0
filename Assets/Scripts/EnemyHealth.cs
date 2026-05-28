using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections; // Necesario para la Corrutina

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] Animator _Animator;
    [SerializeField] Collider _collider; // Arrastra tu collider aquí
    public event Action OnDeath;
    public float _Health;
    public float maxHealth;
    public UnityEngine.AI.NavMeshAgent _EnemyAgent;

    [Header("Loot Settings")]
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 5;
    [SerializeField] private Transform lootspawn;
    [SerializeField] Image HB;
    public bool isDead;
    public AudioSource AS;
    public AudioClip hit;

    void Awake()
    {
        AS = GetComponent<AudioSource>();
        _EnemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _Animator = GetComponentInChildren<Animator>();
    }

    void OnEnable() // Para cuando el objeto sale del Pool vuelva a estar vivo
    {
        isDead = false;
        _Health = maxHealth;
        if(_collider != null) _collider.enabled = true;
        if(HB != null) HB.fillAmount = 1;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Si ya está muerto, no recibe más daño

        AS.PlayOneShot(hit);
        _Health -= damage;
        HB.DOFillAmount(_Health / maxHealth, 0.2f);

        if(_Health <= 0)
        {
            _EnemyAgent.isStopped = true;
            StartCoroutine(HandleDeath()); // Llamamos a la muerte
        }
    }

    IEnumerator HandleDeath()
    {
        isDead = true;
        
        // 1. Desactivar físicas para que no nos estorbe el cadáver
        if(_collider != null) _collider.enabled = false;
        
        // 2. Lanzar la animación
        _Animator.SetTrigger("Death");

        // 3. Opcional: Desactivar el Canvas de vida para que no flote sobre un muerto
        if(HB.transform.parent != null) HB.transform.parent.gameObject.SetActive(false);

        // 4. Esperar un tiempo (ajusta el 2.0f según cuánto dure tu animación)
        yield return new WaitForSeconds(2.0f);

        // 5. Soltar loot
        DropLoot();

        // 6. Avisar y devolver al Pool
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }

    void DropLoot()
    {
        // Generamos un número entre 0 y 100
        float chance = UnityEngine.Random.Range(0f, 100f);

        if (chance <= 70f) 
        {
            // --- LÓGICA DEL 70% (MONEDAS) ---
            int amount = UnityEngine.Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < amount; i++)
            {
                GameObject Coin = PoolManager.Instance.GetPooledObject("Coins", lootspawn.position, lootspawn.rotation);
                if (Coin != null) Coin.SetActive(true);
            }
        }
        else 
        {
            // Asegúrate de que "PocionVida" sea el nombre exacto en tu PoolManager
            GameObject Potion = PoolManager.Instance.GetPooledObject("PocionVida", transform.position, transform.rotation);
            if (Potion != null) Potion.SetActive(true);
            
            Debug.Log("Dropeado: Poción de Vida (30% chance)");
        }
    }
}
