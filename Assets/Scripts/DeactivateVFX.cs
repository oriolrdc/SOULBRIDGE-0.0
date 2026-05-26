using UnityEngine;

public class DeactivateVFX : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("EndThis", 1f);
    }

    void EndThis()
    {
        GameObject nextparent = GameObject.Find("Slashes");
        if(nextparent != null)
        {
            transform.SetParent(nextparent.transform);
        }
        gameObject.SetActive(false);
    }


}
