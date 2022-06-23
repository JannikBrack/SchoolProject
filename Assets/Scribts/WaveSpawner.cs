using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING,WAITING,COUNTING}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform[] enemys;
        public int[] counts;
        public float rate;
    }

    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] Spawnpoits;
    private int nextWave = 0;

    [SerializeField]private float timeBetweenWaves;
    public float waveCountdown;


    public SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                if (!(nextWave + 1 > waves.Length - 1))
                {
                    waveCountdown = timeBetweenWaves;
                    nextWave++;
                    state = SpawnState.COUNTING;
                }
                else return; 
                
            }
            else return;
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private bool EnemyIsAlive()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null) return false;
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for (int n = 0; n < _wave.enemys.Length; n++)
        {
            for (int j = 0; j < _wave.counts[n]; j++)
            {
                SpawnEnemy(_wave.enemys[n]);
                yield return new WaitForSeconds(1f / _wave.rate);
            }
        }
        

        state = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(Transform _enemy)
    {
        int randomNum = Random.Range(0, Spawnpoits.Length);
        Instantiate(_enemy, Spawnpoits[randomNum].position, Spawnpoits[randomNum].rotation);
    }
}
