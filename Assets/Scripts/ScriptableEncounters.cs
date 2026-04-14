using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NuevoEncuentro", menuName = "RPG/Encuentro")]
public class EncounterData : ScriptableObject
{
    public List<WaveData> oleadas;
}
