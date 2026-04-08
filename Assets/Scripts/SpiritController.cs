using UnityEngine;
using UnityEngine.AI;

public class Spirit : MonoBehaviour
{   
    #region Variables
    NavMeshAgent _spiritAgent;
    float _detectionRange = 2f;
    [SerializeField] float _damageArea;
    [SerializeField] LayerMask _enemyMask;
    Transform SelectedEnemy;
    [SerializeField] float distanceToEnemy;
    //StateMachine
    public SpiritState currentSatate;
    #endregion
    #region Awake
    void Awake()
    {
        _spiritAgent = GetComponent<NavMeshAgent>();
    }
    #endregion
    #region SpiritStates
    public enum SpiritState
    {
        Waiting,

        Chasing,

        Attacking
    }
    #endregion
    #region Start
    void Start()
    {
        currentSatate = SpiritState.Chasing;
        SelectedEnemy = NearestEnemy().GetComponent<Transform>();
    }
    #endregion
    #region NearestEnemy
    public GameObject NearestEnemy()
    {
        GameObject selectedEnemy = null;
        float minimumDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float EnemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
            
            if (EnemyDistance < minimumDistance)
            {
                minimumDistance = EnemyDistance;
                selectedEnemy = enemy;
            }
        }

        return selectedEnemy;
    }
    #endregion
    #region Update
    void Update()
    {
        switch(currentSatate)
        {
            case SpiritState.Waiting:
                Waiting();
            break;

            case SpiritState.Chasing:
                Chasing();
            break;

            case SpiritState.Attacking:
                Attacking();
            break;
            default:
                Chasing();
            break;
        }
    }
    #endregion
    #region OnRange
    bool OnRange(float distance)
    {
        if(SelectedEnemy != null)
        {
            distanceToEnemy = Vector3.Distance(transform.position, SelectedEnemy.position);
            Debug.Log(distanceToEnemy);
        }

        if(distanceToEnemy <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region Waiting
    void Waiting()
    {
        if(SelectedEnemy != null)
        {
            currentSatate = SpiritState.Chasing;
        }
    }
    #endregion
    #region Chasing
    void Chasing()
    {
        _spiritAgent.isStopped = false;
        if(SelectedEnemy == null)
        {
            currentSatate = SpiritState.Waiting;
        }
        if(OnRange(_damageArea))
        {
            currentSatate = SpiritState.Attacking;
        }

        if(SelectedEnemy != null)
        {
            _spiritAgent.SetDestination(SelectedEnemy.position);
        }
    }
    #endregion
    #region Attacking
    void Attacking()
    {
        if(!OnRange(_damageArea))
        {
            currentSatate = SpiritState.Chasing;
        }

        _spiritAgent.isStopped = true;
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _damageArea, _enemyMask);
        foreach (Collider enemy in enemiesInRange)
        {
            IDamageable damageable = enemy.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(20);
                Destroy(gameObject);
            }
        }
    }
    #endregion
    #region Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageArea);
    }
    #endregion
}
