using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] GameObject spawnpointParent;
    [SerializeField] List<WaveConfig> premadeEnemyWaves;
    [SerializeField] DynamicWaveConfig repeatingWave;


    private List<Transform> _spawnpoints;
    private int numSpawnpoints;
    private int _lastSpawnpointIndex = 0;
    [SerializeField] int _currentWave = 0;
    public int _numPremadeWaves = 0;
    [SerializeField] bool _waveActive = false;
    [SerializeField] int _activeWaves = 0;
    //private int _difficultyMultiplier = 1;
    public bool GameActive = false;
    private bool _spawning = false;
    [Tooltip("Once preset waves have been exhausted, the chance of spawning special enemies is one in _specialWaveChance - 1")]
    //[SerializeField] int _specialWaveChance = 10;
    [SerializeField] int shopCounter = 0;
    [SerializeField] int shopEveryXWaves = 5;

    private GameSession _gameSession;
    private List<WaveConfig.EnemyInfo> _currentWaveInfo = null;
    private List<DynamicWaveConfig.EnemyInfo> _dynamicCurrentWaveInfo = null;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _gameSession = FindObjectOfType<GameSession>();
        _spawnpoints = GetSpawnpoints();
        numSpawnpoints = _spawnpoints.Count;
        _numPremadeWaves = premadeEnemyWaves.Count;
        //Debug.Log(_numPremadeWaves);
        _gameSession.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameActive)
        {
            if (!_waveActive)
            {
                if (_currentWave < _numPremadeWaves)
                {
                    RunPremadeWaves();
                }
                else
                {
                    //StartWave(premadeEnemyWaves[_numPremadeWaves - 1]);
                    RunDynamicWave();
                }
            }
            else if (_waveActive && transform.childCount == 0 && !_spawning)
            {
                //All enemies have been defeated
                _gameSession.PostWaveActions();
                if (shopCounter >= shopEveryXWaves)
                {
                    SuspendGame();
                    shopCounter = 0;
                }
                else
                {
                    _waveActive = false;
                }
            }
        }
        
    }

    
    //Runs the premade wave in the list of premade waves at index _waveIndex
    private void RunPremadeWaves()
    {
        Debug.Log("Running premade waves");
        StartWave(premadeEnemyWaves[_currentWave]);
        _currentWave++;
    }

    private void RunDynamicWave()
    {
        StartDynamicWave(repeatingWave);
        _currentWave++;
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
        //Debug.Log("Starting wave");
        _waveActive = true;
        _currentWaveInfo = enemyWave.GetEnemiesInWave();
        //Debug.Log(_currentWaveInfo.Count);
        foreach(WaveConfig.EnemyInfo item in _currentWaveInfo)
        {
            //Debug.Log("For eaching");
            //For each struct in the list of structs we will have an enemy prefab and a number X. Spawn that prefab x times
            StartCoroutine(SpawnEnemiesInStruct(item.EnemyPrefab, item.NumEnemies, enemyWave));
        } 
    }

    private void StartDynamicWave(DynamicWaveConfig dynamicWave)
    {
        //Debug.Log("Starting wave");
        _waveActive = true;
        _dynamicCurrentWaveInfo = dynamicWave.GetEnemiesInWave();
        //Debug.Log(_dynamicCurrentWaveInfo.Count);
        foreach (DynamicWaveConfig.EnemyInfo item in _dynamicCurrentWaveInfo)
        {
            //Debug.Log("For eaching");
            int numEnemies = (item.NumEnemies + (int)((item.IncreasePerWave * (_currentWave - _numPremadeWaves)) / 1));
            numEnemies = numEnemies > 0 ? numEnemies : 0;
            //For each struct in the list of structs we will have an enemy prefab and a number X. Spawn that prefab x times
            StartCoroutine(SpawnEnemiesInStruct(item.EnemyPrefab, numEnemies, dynamicWave));
        }
    }


    //Spawns all enemies in the given waveConfig based on its parameters
   IEnumerator SpawnEnemiesInStruct(GameObject enemyPrefab, int numEnemies, WaveConfig waveConfig)
    {
        Debug.Log("Running structs");
        _spawning = true;
        _activeWaves++;
        if (_activeWaves > 1)
        {
            yield return new WaitForSeconds(waveConfig.GetDelayBetweenSpawns() * _activeWaves * 2);
        }
        
        for(int numSpawned = 0; numSpawned < numEnemies; numSpawned++)
        {
            var enemy = Instantiate(enemyPrefab,
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(waveConfig.GetDelayBetweenSpawns() + Random.Range(-waveConfig.GetRandomSpawnOffset(),
                                                                                                waveConfig.GetRandomSpawnOffset()));
        }
        _activeWaves--;
        if(_activeWaves == 0)
        {
            _spawning = false;
            shopCounter++;
        }
    }

    IEnumerator SpawnEnemiesInStruct(GameObject enemyPrefab, int numEnemies, DynamicWaveConfig dynamicWaveConfig)
    {
        Debug.Log("Running structs");
        _spawning = true;
        _activeWaves++;
        if (_activeWaves > 1)
        {
            yield return new WaitForSeconds(dynamicWaveConfig.GetDelayBetweenSpawns() * _activeWaves * 2);
        }

        for (int numSpawned = 0; numSpawned < numEnemies; numSpawned++)
        {
            var enemy = Instantiate(enemyPrefab,
                                    _spawnpoints[FindRandomSpawnpointIndex()].transform.position,
                                    Quaternion.identity);
            enemy.transform.parent = gameObject.transform;
            yield return new WaitForSeconds(dynamicWaveConfig.GetDelayBetweenSpawns() + Random.Range(-dynamicWaveConfig.GetRandomSpawnOffset(),
                                                                                                dynamicWaveConfig.GetRandomSpawnOffset()));
        }
        _activeWaves--;
        if (_activeWaves == 0)
        {
            _spawning = false;
            shopCounter++;
        }
    }

    //Randomly selects one of the spawnpoints to instantiate the current enemy
    private int FindRandomSpawnpointIndex()
    {
        int x = Random.Range(0, _spawnpoints.Count);
        if(x == _lastSpawnpointIndex)
        {
            x = (x + 1) % numSpawnpoints;
        }
        _lastSpawnpointIndex = x;
        return x;
    }


    //Call GameSession.SuspendGame(), set GameActive to false so no more waves are spawned, resets the shopcounter
    private void SuspendGame()
    {
        Debug.Log("Suspending game");
        GameActive = false;
        _gameSession.SuspendGame();

    }

    //Set GameActive to be true and set _waveActive to false to allow for the next waves to spawn
    public void ResumeGame()
    {
        GameActive = true;
        _waveActive = false;
    }
    
}
