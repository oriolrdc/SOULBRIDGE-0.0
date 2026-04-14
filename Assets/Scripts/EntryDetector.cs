using UnityEngine;

public class EntryDetector : MonoBehaviour
{
    public RoomSpawner roomSpawner;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 7)
        {
            roomSpawner.IniciarCombate();
        }
    }
}
