using UnityEngine;

public class PocionController : MonoBehaviour
{
    [SerializeField] SphereCollider collider;
    public AudioSource pocionAS;
    public AudioClip potionSFX;

    void Awake()
    {
        pocionAS = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        GameManager.Instance.Heal();
        pocionAS.PlayOneShot(potionSFX);
        gameObject.SetActive(false);
    }

}
