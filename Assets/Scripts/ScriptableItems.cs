using UnityEngine;

[CreateAssetMenu(fileName = "NuevaMejora", menuName = "Tienda/MejoraArma")]
public class ScriptableItems : ScriptableObject
{
    public string nombre;
    [TextArea] public string descripcion;
    public int price;
    public Sprite icono;
    public Sprite iconoComprada;
    
    [Header("Efectos")]
    public float multiplicador;
    
    public bool comprada = false;
}
