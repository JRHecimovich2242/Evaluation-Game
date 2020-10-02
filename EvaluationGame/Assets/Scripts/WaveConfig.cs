using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject spawnpointPrefab; //This could be unnecessary depending on where we use these waveconfigs
    [SerializeField] float timeBetweenSpawns = 0.75f; //Time between enemy spawns in wave
    [SerializeField] float randomSpawnOffset = 0.3f; //Random factor so that all enemies in a wave arent spaced uniformly
    [SerializeField] int numEnemies = 5;
    [SerializeField] float startDelay = 0f;
    [SerializeField] bool isSpecialWave = false;
    
    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public GameObject GetSpawnpointPrefab() { return spawnpointPrefab; }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetRandomSpawnOffset() { return randomSpawnOffset; }
    public int GetNumberOfEnemies() { return numEnemies; }
    public float GetStartDelay() { return startDelay; }
    public bool GetIsSpecialWave() { return isSpecialWave; }
}
