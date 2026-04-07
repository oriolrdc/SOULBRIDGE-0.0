using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _Speed;
    [SerializeField] float _damage;
    [SerializeField] LayerMask _Layer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rb.linearVelocity = transform.forward * _Speed;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == _Layer)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
            Debug.Log("Choque");
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
