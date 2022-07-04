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
    [SerializeField] Transform[] spawnpoints;

    [SerializeField] int stage1;
    [SerializeField] int stage2;
    [SerializeField] int stage3;
    [SerializeField] int stage4;

    [SerializeField] int nextWave = 0;
    [SerializeField] int savedState = 0;

    [SerializeField] float timeBetweenWaves;
    [SerializeField] float waveCountdown;


    public SpawnState state = SpawnState.COUNTING;

    //sets countdown
    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    
    private void Update()
    {
        //resets complet waves if player dies
        if (playerManager.deadPlayer)
        {
            GameObject[] remainEnemys = FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in remainEnemys)
                if (gameObject.tag == "Enemy" || gameObject.tag == "Item") Destroy(gameObject);
            state = SpawnState.COUNTING;
            nextWave = savedState;
        }
        else
        {
            //complettes the wave if all enemys are dead
            if (state == SpawnState.WAITING)
            {
                if (!playerManager.deadPlayer && !EnemyIsAlive())
                {
                    WaveCompleted();
                    CheckWaveStates();
                }
                else return;
            }

            //if the countdown is 0, it starts a new wave
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

        
    }

    // resets important variables for the next wave
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
            savedState = nextWave - 1;
        }
        
    }

    private bool EnemyIsAlive()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null) return false;
        return true;
    }
    
    //spawning all enemies in the enemy array depending on the parallel array whitch contains the spwan amount
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
        int randomNum = Random.Range(0, 7);
        Instantiate(_enemy, spawnpoints[randomNum].position, spawnpoints[randomNum].rotation);
    }

    //leveling the enemys depending on the completed wave amount
    private void CheckWaveStates()
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
