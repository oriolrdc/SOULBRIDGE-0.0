using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NuevaOleada", menuName = "RPG/Oleada")]
public class WaveData : ScriptableObject
{
    public List<GameObject> enemigosParaSpawnear;
}
