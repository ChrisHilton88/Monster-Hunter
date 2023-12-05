using System.Collections.Generic;
using UnityEngine;

// SO cannot be attached to game objects.
// Template for what a wave is
[CreateAssetMenu(fileName = "NewWave.asset", menuName = "ScriptableObjects/New Wave")]
public class SOEnemyWaves : ScriptableObject
{
    public enum EnemyTypes          // Enum of different Enemy types. To help developers decide which enemies should spawn in each wave.
    {
        Cannibal,
        Warlock,
        Ghoul,
        Tank,
        TreeRoot
    }

    [Header("Wave Properties")]
    [Tooltip("The wave number")]
    public int _waveNumber;
    public float _timeIntervalBetweenEnemies;
    public float _totalRoundTimer;

    [Header("Spawn Sequence")]                    
    [Tooltip("Order in which enemies will spawn, starting with element 0")]
    public List<GameObject> enemyList;            // To be completed by Level Designer through Inspector.
}