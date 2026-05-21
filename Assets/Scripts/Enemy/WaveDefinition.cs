using UnityEngine;

[CreateAssetMenu(fileName = "WaveDefinition", menuName = "BetterWorldGJ/Wave Definition")]
public class WaveDefinition : ScriptableObject
{
    [Min(1)] public int minEnemiesPerBurst = 1;
    [Min(0f)] public float spawnInterval = 2f;
    [Min(0f)] public float delayBetweenSpawnsInBurst = 0.7f;
    [Min(0)] public int waveNumberOffset;
}
