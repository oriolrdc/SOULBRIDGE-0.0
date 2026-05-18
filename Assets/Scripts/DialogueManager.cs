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
    [SerializeField] DialogueData Startdialogo;
    public int Index;

    public void Start()
    {
        dialogueTxt.text = Startdialogo.frases[Index];
        nameTxt.text = Startdialogo.nombre;
        Debug.Log($"Txt: {dialogueTxt}, Data: {Startdialogo}");
        icono.sprite = Startdialogo.icono;
    }

    public void ChangeFrase(DialogueData dialogos)
    {
        if(!Escribiendo)
        {
            if (dialogos.frases.Length == 0) return;
            Index = (Index + 1) % dialogos.frases.Length;
            ActualizarIntefraz(dialogos);
        }
    }

    public void ActualizarIntefraz(DialogueData dialogos)
    {
        dialogueTxt.text = dialogos.frases[Index];
        StartCoroutine(TextAnimation(dialogos));
    }

    public IEnumerator TextAnimation(DialogueData dialogos)
    {
        Escribiendo = true;
        DOTween.Restart("StartText");
        yield return new WaitForSeconds(dialogos.Cooldowns[Index]);
        Escribiendo = false;
    }
}
