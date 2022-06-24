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
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Transform[] Spawnpoits;

    [SerializeField] int stage1;
    [SerializeField] int stage2;
    [SerializeField] int stage3;
    [SerializeField] int stage4;

    [SerializeField] int nextWave = 0;
    [SerializeField] int savedState = 0;

    [SerializeField] float timeBetweenWaves;
    private float waveCountdown;


    public SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (playerManager.deadPlayer)
        {
            savedState = nextWave;
            state = SpawnState.COUNTING;
            waveCountdown = timeBetweenWaves;
            nextWave = 0;
        }
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();   
                CheckWaveStates();
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

    private void WaveCompleted()
    {
        waveCountdown = timeBetweenWaves;
        state = SpawnState.COUNTING;

        if (!(nextWave + 1 < waves.Length - 1))
        {
            return;
            
        }
        else
        {
            nextWave++;
            if (nextWave > savedState) savedState = nextWave;
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
        int randomNum = Random.Range(0, 1);
        Instantiate(_enemy, Spawnpoits[randomNum].position, Spawnpoits[randomNum].rotation);
    }

    private void CheckWaveStates()
    {
        if (savedState == 0)
        {
            if (nextWave < stage1)
            {
                enemyManager.levelSetUp(1);
            }
            else if (nextWave == stage1)
            {
                enemyManager.levelSetUp(2);
            }
            else if (nextWave == stage2)
            {
                enemyManager.levelSetUp(3);
            }
            else if (nextWave == stage3)
            {
                enemyManager.levelSetUp(4);
            }
            else if (nextWave == stage4)
            {
                enemyManager.levelSetUp(5);
            }
        }
        else
        {
            if (savedState < stage1)
            {
                enemyManager.levelSetUp(1);
            }
            else if (savedState == stage1)
            {
                enemyManager.levelSetUp(2);
            }
            else if (savedState == stage2)
            {
                enemyManager.levelSetUp(3);
            }
            else if (savedState == stage3)
            {
                enemyManager.levelSetUp(4);
            }
            else if (savedState == stage4)
            {
                enemyManager.levelSetUp(5);
            }
        }
    }
}
