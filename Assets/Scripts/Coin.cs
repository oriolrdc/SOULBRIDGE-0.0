using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] SphereCollider collider;
    public AudioSource coinAS;
    public AudioClip coinsfx;

    void Start()
    {
        coinAS = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        GameManager.Instance.AddCoins();
        coinAS.PlayOneShot(coinsfx);
        gameObject.SetActive(false);

    }

    void ActivateCollider()
    {
        collider.enabled = true;
    }
}
