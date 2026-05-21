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
    public bool DialogueStarted;
    public GameObject DCanvas;
    public int Index;
    public GameObject card;

    public void Start()
    {
        Escribiendo = false;
        dialogueTxt.text = Startdialogo.frases[Index].lafrase;
        nameTxt.text = Startdialogo.frases[Index].nombrePersonaje;
        Debug.Log($"Txt: {dialogueTxt}, Data: {Startdialogo}");
        icono.sprite = Startdialogo.frases[Index].Icono;
    }

    public void IniciarDialogo(DialogueData dialogos)
    {
        Index = 0;
        ActualizarIntefraz(dialogos);
    }

    public void ChangeFrase(DialogueData dialogos)
    {
        if(!Escribiendo)
        {
            // Primero comprobamos si es la última frase ANTES de sumar
            if (Index >= dialogos.frases.Length - 1)
            {
                DCanvas.SetActive(false);
                GameManager.Instance.ADInputs();
                DialogueStarted = false;
                Destroy(card);
                return;
            }

            Index++;
            ActualizarIntefraz(dialogos);
        }
    }

    public void ActualizarIntefraz(DialogueData dialogos)
    {
        // Borra la línea que pone el texto directamente
        StartCoroutine(TextAnimation(dialogos));
    }

    public IEnumerator TextAnimation(DialogueData dialogos)
    {
        Escribiendo = true;

        dialogueTxt.text = "";
        nameTxt.text = "";

        float duracion = dialogos.Cooldowns[Index];
        
        dialogueTxt.DOText(dialogos.frases[Index].lafrase, duracion).SetEase(Ease.Linear);
        nameTxt.DOText(dialogos.frases[Index].nombrePersonaje, duracion).SetEase(Ease.Linear);
        icono.sprite = dialogos.frases[Index].Icono;

        yield return new WaitForSeconds(duracion);

        Escribiendo = false;
    }
}
