using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSpawner : MonoBehaviour
{
    public EncounterData datosEncuentro; // Arrastramos aquí nuestro ScriptableObject
    public Transform[] spawnPoints;      // Puntos vacíos colocados por la sala
    
    private int oleadaActual = 0;
    private List<GameObject> enemigosActivos = new List<GameObject>();
    public GameObject exitTrigger;

    public void IniciarCombate() 
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // 1. Bloqueamos el chequeo de victoria
        bool oleadaSpawneando = true; 
        WaveData wave = datosEncuentro.oleadas[oleadaActual];

        foreach (GameObject prefab in wave.enemigosParaSpawnear)
        {
            Transform punto = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemigo = Instantiate(prefab, punto.position, Quaternion.identity);
            
            enemigosActivos.Add(enemigo);
            
            // Nos suscribimos a su muerte
            enemigo.GetComponent<EnemyHealth>().OnDeath += () => VerificaryLimpiar(enemigo);

            yield return new WaitForSeconds(0.5f); 
        }

        // 2. Ya han salido todos, ahora sí permitimos pasar de oleada
        oleadaSpawneando = false;
        
        // Chequeo de seguridad por si todos murieron mientras spawneaban
        if(enemigosActivos.Count == 0) SiguienteOleada();
    }

    void VerificaryLimpiar(GameObject enemigoMuerto)
    {
        if (enemigosActivos.Contains(enemigoMuerto))
        {
            enemigosActivos.Remove(enemigoMuerto);
        }

        // Solo pasamos de oleada si la lista está vacía Y ya terminamos de instanciar
        if (enemigosActivos.Count == 0)
        {
            SiguienteOleada();
        }
    }

    void SiguienteOleada()
    {
        oleadaActual++;
        if (oleadaActual < datosEncuentro.oleadas.Count)
        {
            StartCoroutine(SpawnWave());
        }
        else
        {
            Debug.Log("SALA COMPLETADA! Aparece el cofre.");
            exitTrigger.SetActive(true);
        }
    }
}
