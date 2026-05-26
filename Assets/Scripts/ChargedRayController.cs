using UnityEngine;

public class ChargedRayController : MonoBehaviour
{
    
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _Speed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = transform.forward * _Speed;
    }

    void OnEnable()
    {
        Invoke("MuerteSubida", 3f);
    }

    void MuerteSubida()
    {
        gameObject.SetActive(false);
    }
}
