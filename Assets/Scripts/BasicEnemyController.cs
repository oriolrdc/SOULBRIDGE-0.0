using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class BasicEnemyController : MonoBehaviour, IKnockbackable
{
    [SerializeField] CapsuleCollider _collider;
    [SerializeField] Animator _Animator;
    [SerializeField] EnemyHealth EH;
    NavMeshAgent _EnemyAgent;
    [SerializeField] Transform _player;
    [SerializeField] float _detectionRange;
    [SerializeField] float _damageArea;
    [SerializeField] LayerMask _playerMask;
    public EnemyState currentSatate;
    private float attackCooldown = 1;
    private float timer;
    private float stunTimer;

    void Awake()
    {
        _EnemyAgent = GetComponent<NavMeshAgent>();
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
        _player = GameObject.FindWithTag("Player").transform;
        currentSatate = EnemyState.Chasing;
        timer = 1;
    }
    
    void Update()
    {
        switch(currentSatate)
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
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if(distanceToPlayer <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    void Waiting()
    {
        if(EH.isDead) return;
        if(_player != null)
        {
            currentSatate = EnemyState.Chasing;
        }
        _Animator.SetFloat("Speed", 0);
    }
    
    void Chasing()
    {
        if(EH.isDead) return;

        if(_player == null)
        {
            currentSatate = EnemyState.Waiting;
            return;
        }

        if(OnRange(_detectionRange))
        {
            _EnemyAgent.isStopped = false;
            _EnemyAgent.SetDestination(_player.position);
            _Animator.SetFloat("Speed", 1);

            if(OnRange(_damageArea))
            {
                currentSatate = EnemyState.Attacking;
            }
        }
        else
        {
            _EnemyAgent.isStopped = true;
            _Animator.SetFloat("Speed", 0);
        }
    }
    
    void Attacking()
    {
        if(EH.isDead) return;
        if(!OnRange(_damageArea))
        {
            currentSatate = EnemyState.Chasing;
        }

        IDamageable damageable = _player.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            if(timer >= attackCooldown)
            {
                damageable.TakeDamage(5);
                Debug.Log("damage");
                timer = 0;
            }

            timer += Time.deltaTime;
        }
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
        _Animator.SetTrigger("Hit");
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

    public void Dead()
    {
        _Animator.SetTrigger("Death");
        _collider.enabled = false;
        _EnemyAgent.isStopped = true;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageArea);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
