﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dynamic Enemy Wave Config")]
public class DynamicWaveConfig : ScriptableObject
{
    [System.Serializable]
    public struct EnemyInfo
    {
        public GameObject EnemyPrefab;
        public int NumEnemies;
        public float IncreasePerWave;

    }
    [SerializeField] List<EnemyInfo> enemiesInWave;
    [SerializeField] float _delayBetweenSpawns = .5f;
    [SerializeField] float _randomSpawnOffset = .3f;



    public List<EnemyInfo> GetEnemiesInWave() { return enemiesInWave; }

    public float GetDelayBetweenSpawns() { return _delayBetweenSpawns; }

    public float GetRandomSpawnOffset() { return _randomSpawnOffset; }

}
