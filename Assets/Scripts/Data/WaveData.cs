using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Create/Wave Data")]
public class WaveData : ScriptableObject
{
    public int reward;
    public List<WaveEntry> waveEntries = new List<WaveEntry>();
}

[Serializable]
public class WaveEntry
{
    public Enemy Enemy;
    public Vector2 spawnTime;
    public Vector2 health;
}
