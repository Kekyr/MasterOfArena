using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int CurrentPointIndex;
    public float StartBarValue;
    public float EndBarValue;
    public int Score;
    public int CurrentZoneIndex;
    public List<float> SpawnChances;
    public bool CanPlay;

    public SaveData(int currentPointIndex, float startBarValue, float endBarValue, int score, int currentZoneIndex,
        List<float> spawnChances, bool canPlay)
    {
        CurrentPointIndex = currentPointIndex;
        StartBarValue = startBarValue;
        EndBarValue = endBarValue;
        Score = score;
        CurrentZoneIndex = currentZoneIndex;
        SpawnChances = spawnChances;
        CanPlay = canPlay;
    }
}