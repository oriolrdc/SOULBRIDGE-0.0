using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _Speed;
    [SerializeField] float _Layer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == _Layer)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(PlayerData.Instance.BulletDamage);
            }
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
