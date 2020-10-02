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
    [SerializeField] WaveConfig oneHeavyEnemy;
    [SerializeField] WaveConfig fiveFastEnemies;


    private List<Transform> _spawnpoints;
    //private int _lastUsedSpawnpoint = -1;
    [SerializeField] int _activeWaves = 0;
    [SerializeField] int _waveIndex = 9;
    private int _numPremadeWaves = 0;
    private int _numSpecialWaves = 0;
    [SerializeField] int _targetNumEnemies = 10;
    [SerializeField] int _spawnedEnemies = 0;
    [SerializeField] bool _waveActive = false;
    private bool _cooldownActive = false;
    [SerializeField] bool _spawning = false;
    private float heavyWaveStartTime = 0f;
    private float heavyWaveCooldown = 30f;
    private float fastWaveStartTime = 0f;
    private float fastWaveCooldown = 30f;
    private int _maxEnemiesOnScreen = 10;
    public bool GameActive = false;
    [SerializeField] int allowedActiveWaves = 1;
    [SerializeField] float delayBetweenWaves = 5f;
    [SerializeField] int waveLoops = 0;

    private GameSession _gameSession;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _gameSession = FindObjectOfType<GameSession>();
        _spawnpoints = GetSpawnpoints();
        _numPremadeWaves = premadeEnemyWaves.Count;
        _numSpecialWaves = specialEnemyWaves.Count;
        _gameSession.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameActive)
        {
            if (_waveIndex < _numPremadeWaves)
            {
                RunPremadeWaves();
            }
            else
            {
                //Debug.Log("Out of intro waves");
                //BEFORE YOU START TRYING TO MAKE SPECIAL ENEMIES AND WAVES, MAKE SURE THE GAME WORKS WITH JUST BASIC ENEMIES AND WEAPONS. 
                //Want to run waves incrementing by 5 enemies each time (Will this be too hard?)
                //If we want to spawn 20 enemies, we spawn waves of 10 twice
                //Sprinkle in some special enemy waves too
                //RunDynamicWaves();
                _waveIndex = 0;
                waveLoops++;

            }
        }
        else if(_waveActive && !GameActive)
        {
            //(hopefully) Stop enemies from spawning if player dies mid wave
            StopAllCoroutines();
        }
    }

    private void RunPremadeWaves()
    {
        if (!_waveActive)
        {
            StartWave(premadeEnemyWaves[_waveIndex]);
        }
    }

    private void RunDynamicWaves()
    {
        //OriginalRunDynamicWaves();
        if (gameObject.transform.childCount <= _maxEnemiesOnScreen)
        {
            if (!_spawning)
            {
                StartWave(fiveEnemyWave);
            }
            int roll = Random.Range(0, 10);
            if (roll <= 2 && Time.time - heavyWaveStartTime >= heavyWaveCooldown)
            {
                heavyWaveStartTime = Time.time;
                StartWave(oneHeavyEnemy);
            }
            else if (roll >= 7 && Time.time - fastWaveStartTime >= fastWaveCooldown)
            {
                fastWaveStartTime = Time.time;
                StartWave(fiveFastEnemies);
            }
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
        _waveIndex++;
        _spawning = true;
        //Debug.Log("Starting a wave");
        _activeWaves++;
        _waveActive = true;
        _gameSession.UpdateCurrentWave(_waveIndex);
        StartCoroutine(SpawnEnemiesInWave(enemyWave));
        if (enemyWave.GetIsSpecialWave())
        {
            if(waveLoops > 0)
            {
                //After first loop we want to start piling regular enemies on top of special enemies
                StartCoroutine(SpawnEnemiesInWave(fiveEnemyWave));
            }
            
        }
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
        //Debug.Log("Booling");
        for (int enemiesSpawned = 0; 
            enemiesSpawned < waveConfig.GetNumberOfEnemies(); 
            enemiesSpawned++)
        {
            //Debug.Log("Spawning an enemy");
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(),
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            //Debug.Log("BOOLING");
            _spawnedEnemies++;
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns() + Random.Range(-waveConfig.GetRandomSpawnOffset(), 
                                                                                                waveConfig.GetRandomSpawnOffset()));
        }
        _spawning = false;
        _cooldownActive = true;
        StartCoroutine(WaveCooldown());
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
        //_gameSession.UpdateCurrentWave(_waveIndex);
        _spawnedEnemies = 0;
    }

    private void OriginalRunDynamicWaves()
    {
        //Debug.Log("Zoom");
        if (_waveActive)
        {
            if (gameObject.transform.childCount == 0 && !_cooldownActive && (_spawnedEnemies >= _targetNumEnemies))
            {
                Debug.Log("Trying to cooldown");
                _cooldownActive = true;
                StartCoroutine(WaveCooldown());
            }
        }
        if (_activeWaves == 0 || _activeWaves < allowedActiveWaves)
        {
            Debug.Log("Trying to spawn");
            allowedActiveWaves = _targetNumEnemies / 5;
            if (_spawnedEnemies < _targetNumEnemies && !_spawning)
            {
                Debug.Log("Start this wave");
                StartWave(fiveEnemyWave);
                Debug.Log(_spawnedEnemies);

            }
        }
    }
}
