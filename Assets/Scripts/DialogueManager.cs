using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Text dialogueTxt;
    [SerializeField] Text nameTxt;
    [SerializeField] Image icono;
    [SerializeField] bool Escribiendo;

    public void ChangeFrase()
    {
        
    }

    public IEnumerator TextAnimation()
    {
        Escribiendo = true;
        DOTween.Restart("StartText");
        yield return new WaitForSeconds(2);
        Escribiendo = false;
    }
}
