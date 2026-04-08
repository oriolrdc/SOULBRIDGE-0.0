using UnityEngine;

public class DoorController : MonoBehaviour
{    
    [SerializeField] string nextScene;
    [SerializeField] float _playerMask;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == _playerMask)
        {
            GameManager.Instance.EndLevel(nextScene);
        }
    }
}
