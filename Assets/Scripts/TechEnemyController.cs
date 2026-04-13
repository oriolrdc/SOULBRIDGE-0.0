using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class TechEnemy : MonoBehaviour, IKnockbackable
{
    [SerializeField] CapsuleCollider _collider;
    [SerializeField] float _movementSpeed = 3.5f;
    UnityEngine.AI.NavMeshAgent _EnemyAgent;
    [SerializeField] Transform _player;
    [SerializeField] float _detectionRange;
    [SerializeField] float _attackArea;
    [SerializeField] LayerMask _playerMask;
    public EnemyState currentSatate;
    private float attackCooldown = 2;
    private float timer;
    private float stunTimer;

    // --- Nuevas variables para la embestida ---
    [Header("Settings de Ataque")]
    [SerializeField, Range(0, 100)] float _heavyAttackChance = 30f; // Probabilidad de ataque fuerte
    [SerializeField] float _dashSpeed = 12f;
    private bool _isDashing = false;

    void Awake()
    {
        _EnemyAgent = GetComponent<NavMeshAgent>();
        _EnemyAgent.speed = _movementSpeed;
    }

    public enum EnemyState
    {
        Waiting,
        
        Chasing,
        
        Attacking,
        
        Stunned
    }

    void Start()
    {
        if (_player == null) _player = GameObject.FindWithTag("Player").transform;
        currentSatate = EnemyState.Chasing;
    }

    void Update()
    {
        switch (currentSatate)
        {
            case EnemyState.Waiting: 
                Waiting();
            break;
            
            case EnemyState.Chasing: 
                Chasing();
            break;
            
            case EnemyState.Attacking: 
                Attacking();
            break;

            case EnemyState.Stunned:
                HandleStun();
            break;
            
            default: 
                Chasing();
            break;
        }
    }

    bool OnRange(float distance)
    {
        if (_player == null) return false;
        return Vector3.Distance(transform.position, _player.position) <= distance;
    }

    void Waiting()
    {
        if (_player != null) currentSatate = EnemyState.Chasing;
    }

    void Chasing()
    {
        if (_player == null)
        {
            currentSatate = EnemyState.Waiting;
            return;
        }

        if (OnRange(_detectionRange))
        {
            _EnemyAgent.SetDestination(_player.position);
        }

        if (OnRange(_attackArea))
        {
            currentSatate = EnemyState.Attacking;
        }
    }

    void Attacking()
    {
        // Si no estamos embistiendo y el jugador se aleja, volvemos a perseguir
        if (!OnRange(_attackArea) && !_isDashing)
        {
            currentSatate = EnemyState.Chasing;
            return;
        }

        if (timer >= attackCooldown)
        {
            // Decidir tipo de ataque
            float decision = Random.Range(0, 100);
            if (decision <= _heavyAttackChance)
            {
                StartCoroutine(PerformDashAttack());
            }
            else
            {
                PerformBasicAttack();
            }
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    void PerformBasicAttack()
    {
        Debug.Log("Ataque Básico");
        ApplyDamageToPlayer(10);
    }

    System.Collections.IEnumerator PerformDashAttack()
    {
        _isDashing = true;
        
        // Pequeña pausa de "carga" antes de salir disparado
        _EnemyAgent.isStopped = true;
        yield return new WaitForSeconds(0.5f); 
        
        // Configurar velocidad de embestida
        _EnemyAgent.isStopped = false;
        _EnemyAgent.speed = _dashSpeed;
        _EnemyAgent.acceleration = 100f; // Aceleración instantánea
        
        // Apuntar a la posición donde ESTABA el jugador al empezar la carga
        Vector3 dashTarget = _player.position;
        _EnemyAgent.SetDestination(dashTarget);

        // Esperar a que llegue o pase un tiempo máximo
        float dashTimer = 0;
        while (dashTimer < 0.6f && _EnemyAgent.remainingDistance > 0.5f)
        {
            dashTimer += Time.deltaTime;
            yield return null;
        }

        // Aplicar daño si al terminar la carga sigue cerca
        if (OnRange(2f)) ApplyDamageToPlayer(25);

        // Restaurar valores originales
        _EnemyAgent.speed = _movementSpeed;
        _EnemyAgent.acceleration = 8f; // Valor por defecto de NavMeshAgent
        _isDashing = false;
        Debug.Log("Embestida Terminada");
    }

    void ApplyDamageToPlayer(float damage)
    {
        if (_player == null) return;
        IDamageable damageable = _player.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(damage);
    }

    void HandleStun()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
        {
            _EnemyAgent.enabled = true;
            currentSatate = EnemyState.Chasing;
        }
    }

    public void ApplyKnockback(Vector3 force, float duration)
    {
        currentSatate = EnemyState.Stunned;
        stunTimer = duration;
        _EnemyAgent.enabled = false;
        StartCoroutine(KnockbackRoutine(force, duration));
    }

    IEnumerator KnockbackRoutine(Vector3 force, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            Vector3 frameForce = Vector3.Lerp(force, Vector3.zero, elapsed / duration);
            transform.position += frameForce * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackArea);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
