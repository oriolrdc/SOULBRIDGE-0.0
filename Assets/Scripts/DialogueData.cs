using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FraseDialogo
{
    public string nombrePersonaje;
    [TextArea(3, 10)] public string lafrase;
    public Sprite Icono;
}

[CreateAssetMenu(fileName = "Nuevo dialogo", menuName = "Dialogos")]
public class DialogueData : ScriptableObject
{
    public FraseDialogo[] frases;
    public float[] Cooldowns;
}
