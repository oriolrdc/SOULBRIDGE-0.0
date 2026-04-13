using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] SphereCollider collider;

    void OnTriggerEnter(Collider collider)
    {

        GameManager.Instance.AddCoins();
        gameObject.SetActive(false);

    }

    void ActivateCollider()
    {
        collider.enabled = true;
    }
}
