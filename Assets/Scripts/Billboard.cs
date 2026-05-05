using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        // Busca la cámara principal
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Hace que el objeto mire a la cámara
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);
    }
}

