using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemySpawn;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;

    private float timecount = 2f;
    private int waveNumber = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countdown <= 0f)
        {
            SpawnWave();
            timecount = timeBetweenWaves;
        }
        timecount -= Time.deltaTime;
    }

    private void SpawnWave()
    {
        for(int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
        }
        waveNumber++;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemySpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
