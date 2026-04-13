using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NuevaOleada", menuName = "RPG/Oleada")]
public class WaveData : ScriptableObject
{
    // Una lista simple de los prefabs de enemigos que quieres en esta oleada
    public List<GameObject> enemigosParaSpawnear;
}

[CreateAssetMenu(fileName = "NuevoEncuentro", menuName = "RPG/Encuentro")]
public class EncounterData : ScriptableObject
{
    public List<WaveData> oleadas;
}
