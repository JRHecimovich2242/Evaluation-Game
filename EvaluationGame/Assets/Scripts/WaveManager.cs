using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject spawnpointParent;
    [SerializeField] List<WaveConfig> premadeEnemyWaves;
    [SerializeField] List<WaveConfig> specialEnemyWaves;
    [SerializeField] WaveConfig tenEnemyWave;
    [SerializeField] WaveConfig fiveEnemyWave;

    private List<Transform> _spawnpoints;
    private int _lastUsedSpawnpoint = -1;
    private int _activeWaves = 0;
    private int _waveIndex = 0;
    private int _numPremadeWaves = 0;
    private bool _waveActive = false;
    private bool _cooldownActive = false;
    [SerializeField] int allowedActiveWaves = 1;
    [SerializeField] float delayBetweenWaves = 5f;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _spawnpoints = GetSpawnpoints();
        _numPremadeWaves = premadeEnemyWaves.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if(_waveIndex <= _numPremadeWaves)
        {
            RunPremadeWaves();
        }
        else
        {
            Debug.Log("Out of intro waves");
            //BEFORE YOU START TRYING TO MAKE SPECIAL ENEMIES AND WAVES, MAKE SURE THE GAME WORKS WITH JUST BASIC ENEMIES AND WEAPONS. 
            //Want to run waves incrementing by 5 enemies each time (Will this be too hard?)
            //If we want to spawn 20 enemies, we spawn waves of 10 twice
            //Sprinkle in some special enemy waves too
        }
        
    }

    private void RunPremadeWaves()
    {
        if (_waveActive)
        {
            if (gameObject.transform.childCount == 0 && !_cooldownActive)
            {
                _cooldownActive = true;
                StartCoroutine(WaveCooldown());
            }
        }
        if (_activeWaves == 0 || _activeWaves < allowedActiveWaves)
        {

            //Introduce a wave cooldown between waves or should it be nonstop?
            StartWave(premadeEnemyWaves[_waveIndex]);
            _waveIndex++;
            //_waveIndex = _waveIndex % _numWaves;
        }
    }

    private List<Transform> GetSpawnpoints()
    {
        var spawnpoints = new List<Transform>();
        foreach(Transform child in spawnpointParent.transform)
        {
            spawnpoints.Add(child);
        }
        return spawnpoints;
    }

    public void StartWave(WaveConfig enemyWave)
    {
        Debug.Log("Starting a wave");
        _activeWaves++;
        _waveActive = true;
        StartCoroutine(SpawnEnemiesInWave(enemyWave));
    }

    public void StartWave(WaveConfig waveConfig, WaveConfig specialWave)
    {

    }

    public void EndWave()
    {
        //We can instantiate all enemies as a child of the WaveManager or GameManager.
        //Hopefully there is a constant time operation to check for children of a gameObject
        //If no children, end wave
    }

    private IEnumerator SpawnEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemiesSpawned = 0; 
            enemiesSpawned < waveConfig.GetNumberOfEnemies(); 
            enemiesSpawned++)
        {
            Debug.Log("Spawning an enemy");
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(),
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns() + Random.Range(-waveConfig.GetRandomSpawnOffset(), 
                                                                                                waveConfig.GetRandomSpawnOffset()));
        }
        
    }

    //Maybe make an overload for SpawnEnemiesInWave that takes the WaveConfig and a delay float. We only need one spawn coroutine because we can have multiple waves spawning at once
    //With a delay float, we can have either overlapping waves OR two waves at once where the second wave is scattered.

    private IEnumerator SpawnEnemiesInWave(WaveConfig standardWave, WaveConfig specialWave)
    {
        for (int enemiesSpawned = 0;
            enemiesSpawned < (standardWave.GetNumberOfEnemies() + specialWave.GetNumberOfEnemies());
            enemiesSpawned++)
        {
            var enemy = Instantiate(standardWave.GetEnemyPrefab(),
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;

            yield return new WaitForSeconds(standardWave.GetTimeBetweenSpawns() + Random.Range(-standardWave.GetRandomSpawnOffset(),
                                                                                                standardWave.GetRandomSpawnOffset()));
        }
    }

    private int FindRandomSpawnpointIndex()
    {
        return Random.Range(0, _spawnpoints.Count);
    }

    private IEnumerator WaveCooldown()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        _waveActive = false;
        _activeWaves = 0;
        _cooldownActive = false;
        _gameManager.UpdateCurrentWave(_waveIndex);
    }
}
