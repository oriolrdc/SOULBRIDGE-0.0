using UnityEngine;

public class EntryDetector : MonoBehaviour
{
    public RoomSpawner roomSpawner;
    public bool Started;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 7 && !Started)
        {
            Started = true;
            roomSpawner.IniciarCombate();
        }
    }
}
