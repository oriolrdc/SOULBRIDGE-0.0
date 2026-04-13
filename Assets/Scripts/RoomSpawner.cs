using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomSpawner : MonoBehaviour
{
    public EncounterData datosEncuentro;
    public Transform[] spawnPoints;
    
    private int oleadaActual = 0;
    private List<GameObject> enemigosActivos = new List<GameObject>();
    public GameObject exitTrigger;
    public GameObject vfxSpawnCircle;
    public float tiempoDeAviso = 1.2f;
    public bool estaSpawneando;

    public void IniciarCombate() 
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        estaSpawneando = true;
        WaveData wave = datosEncuentro.oleadas[oleadaActual];

        foreach (GameObject prefabEnemigo in wave.enemigosParaSpawnear)
        {
            Transform punto = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // 1. Lanzamos el proceso de spawn individual para cada enemigo
            StartCoroutine(ProcesoSpawnEnemigo(prefabEnemigo, punto.position));

            // Espera entre un enemigo y otro de la misma oleada
            yield return new WaitForSeconds(0.5f);
        }

        estaSpawneando = false;
    }

    IEnumerator ProcesoSpawnEnemigo(GameObject prefab, Vector3 posicion)
    {
        // 2. Aparece el círculo de aviso
        if (vfxSpawnCircle != null)
        {
            Instantiate(vfxSpawnCircle, posicion, Quaternion.identity);
        }

        // 3. Esperamos el tiempo de carga del ataque/aparición
        yield return new WaitForSeconds(tiempoDeAviso);

        // 4. Instanciamos al enemigo real
        GameObject enemigo = Instantiate(prefab, posicion, Quaternion.identity);
        
        enemigosActivos.Add(enemigo);
        enemigo.GetComponent<EnemyHealth>().OnDeath += () => VerificaryLimpiar(enemigo);
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
