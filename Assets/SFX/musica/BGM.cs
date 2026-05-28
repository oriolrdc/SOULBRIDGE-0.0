using UnityEngine;

public class BGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            AudioSource myAudio = GetComponent<AudioSource>();
            myAudio.Play();
        }
    }
}
