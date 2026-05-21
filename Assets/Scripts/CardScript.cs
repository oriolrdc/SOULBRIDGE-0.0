using UnityEngine;

public class CardScript : MonoBehaviour
{
    public DialogueManager Dscript;
    public DialogueData primerosDialogos;
    public BoxCollider coll;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && !Dscript.DialogueStarted)
        {
            Dscript.DialogueStarted = true;
            GameManager.Instance.ADInputs();
            Dscript.DCanvas.SetActive(true);
            coll.enabled = false;
            Dscript.IniciarDialogo(primerosDialogos);
        }
    }
}
