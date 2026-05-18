using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Nuevo dialogo", menuName = "Dialogos")]
public class DialogueData : ScriptableObject
{
    public string[] frases;
    public string nombre;
    public Sprite icono;
    public float[] Cooldowns;
}
