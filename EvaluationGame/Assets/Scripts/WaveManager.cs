using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] GameObject spawnpointParent;
    [SerializeField] List<WaveConfig> premadeEnemyWaves;
    [SerializeField] WaveConfig fiveEnemyWave;
    [SerializeField] WaveConfig oneHeavyEnemy;
    [SerializeField] WaveConfig fiveFastEnemies;


    private List<Transform> _spawnpoints;
    [SerializeField] int _currentWave = 0;
    public int _numPremadeWaves = 0;
    [SerializeField] bool _waveActive = false;
    private int _difficultyMultiplier = 1;
    public bool GameActive = false;
    [SerializeField] float delayBetweenWaves = 5f;
    private bool _coroutineActive = false;
    private int _difficultyIncrement = 0;
    [Tooltip("Once preset waves have been exhausted, the chance of spawning special enemies is one in _specialWaveChance - 1")]
    [SerializeField] int _specialWaveChance = 10;
    [SerializeField] int shopCounter = 0;
    [SerializeField] int shopEveryXWaves = 5;

    private GameSession _gameSession;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _gameSession = FindObjectOfType<GameSession>();
        _spawnpoints = GetSpawnpoints();
        _numPremadeWaves = premadeEnemyWaves.Count;
        //Debug.Log(_numPremadeWaves);
        _gameSession.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (shopCounter >= shopEveryXWaves && transform.childCount == 0)
        {
            //Run suspension for store
            SuspendGame();
        }
        if (GameActive && shopCounter <= shopEveryXWaves)
        {
            if (_currentWave < _numPremadeWaves)
            {
                RunPremadeWaves();
            }
            else
            {
                //Start running repeat waves
                RunRandomWaves();
            }
        }
        else if(_waveActive && !GameActive)
        {
            //(hopefully) Stop enemies from spawning if player dies mid wave
            StopAllCoroutines();
        }
    }

    //Runs the premade wave in the list of premade waves at index _waveIndex
    private void RunPremadeWaves()
    {
        if (!_waveActive)
        {
            StartWave(premadeEnemyWaves[_currentWave]);
            _currentWave++;
        }
    }

    //Once all premade waves have been exhausted, RunRandomWaves regularly runs five enemy waves, and multiplies the number of enemies as time passes.
    private void RunRandomWaves()
    {
        if (!_waveActive)
        {
            //Debug.Log("Running a random wave");
            _currentWave++;
            _difficultyIncrement++;
            StartWave(fiveEnemyWave, _difficultyMultiplier);
            //Debug.Log("Spawning " + _difficultyMultiplier.ToString() + " Five enemy waves");
            int randFactor = Random.Range(0, _specialWaveChance);
            //Debug.Log(randFactor);
            if (randFactor == 1)
            {
                StartWave(fiveFastEnemies, _difficultyMultiplier - 1);
                //Debug.Log("Spawning " + (_difficultyMultiplier - 1).ToString() + " Five fast enemy waves");
            }
            else if (randFactor == 0)
            {
                StartWave(oneHeavyEnemy, _difficultyMultiplier);
                //Debug.Log("Spawning " + _difficultyMultiplier.ToString() + " One Heavy enemy waves");
            }
        }
        if(_difficultyIncrement == 5)
        {
            _difficultyMultiplier++;
            _difficultyIncrement = 0;
            if(_specialWaveChance > 2)
            {
                _specialWaveChance--;
            }
        }
    }

   
    //Adds all spawnpoints to a list for easy reference
    private List<Transform> GetSpawnpoints()
    {
        var spawnpoints = new List<Transform>();
        foreach(Transform child in spawnpointParent.transform)
        {
            spawnpoints.Add(child);
        }
        return spawnpoints;
    }

    //Starts the given wave
    public void StartWave(WaveConfig enemyWave)
    {
        //Debug.Log("Starting a wave");
        _waveActive = true;
        _gameSession.UpdateCurrentWave(_currentWave);
        StartCoroutine(SpawnEnemiesInWave(enemyWave));
    }

    //Starts the given wave, also takes a spawnMultiplier to be passed to SpawnEnemiesInWave
    public void StartWave(WaveConfig enemyWave, int spawnMultiplier)
    {
        //Debug.Log("Starting a wave");
        _waveActive = true;
        _gameSession.UpdateCurrentWave(_currentWave);
        StartCoroutine(SpawnEnemiesInWave(enemyWave, spawnMultiplier));
    }

    //Spawns all enemies in the given waveConfig based on its parameters
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
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns() + Random.Range(-waveConfig.GetRandomSpawnOffset(), 
                                                                                                waveConfig.GetRandomSpawnOffset()));
        }
        StartCoroutine(WaveCooldown());
    }

    //Spawns all enemies in the given waveConfig spawnMultiplier times
    private IEnumerator SpawnEnemiesInWave(WaveConfig waveConfig, int spawnMultiplier)
    {
        for (int enemiesSpawned = 0;
            enemiesSpawned < (waveConfig.GetNumberOfEnemies() * spawnMultiplier);
            enemiesSpawned++)
        {
            //Debug.Log("Spawning an enemy");
            var enemy = Instantiate(waveConfig.GetEnemyPrefab(),
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns() + Random.Range(-waveConfig.GetRandomSpawnOffset(),
                                                                                                waveConfig.GetRandomSpawnOffset()));
        }
        StartCoroutine(WaveCooldown());
    }

    //Randomly selects one of the spawnpoints to instantiate the current enemy
    private int FindRandomSpawnpointIndex()
    {
        return Random.Range(0, _spawnpoints.Count);
    }

    //Once a wave has finished, WaveCooldown begins a timer until the next set of waves can begin
    private IEnumerator WaveCooldown()
    {
        //Only allow one cooldown coroutine to be active at a time
        if (!_coroutineActive)
        {
            
            _coroutineActive = true;
            yield return new WaitForSeconds(delayBetweenWaves);
            //_gameSession.UpdateCurrentWave(_waveIndex);
            _coroutineActive = false;
            shopCounter++;
            if (shopCounter <= shopEveryXWaves)
            {
                //When _waveActive is false, the WaveManager will begin another wave. If it is time for the shop to spawn, we dont want another wave to appear until it is gone
                //So _waveActive is only updated when shopCounter <= shopEveryXWaves
                _waveActive = false;
            }
            
        }
    }

    //Call GameSession.SuspendGame(), set GameActive to false so no more waves are spawned, resets the shopcounter
    private void SuspendGame()
    {
        GameActive = false;
        _gameSession.SuspendGame();
        shopCounter = 0;

    }

    //Set GameActive to be true and set _waveActive to false to allow for the next waves to spawn
    public void ResumeGame()
    {
        GameActive = true;
        _waveActive = false;
    }
}
