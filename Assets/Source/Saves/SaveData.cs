using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class SaveData
{
    private readonly int defaultCurrentPointIndex = 0;
    private readonly float defaultStartBarValue = 0;
    private readonly float defaultEndBarValue = 0.2f;

    private readonly int defaultScore = 0;
    private readonly int defaultCoins = 0;
    private readonly int defaultCurrentZoneIndex = 0;

    private readonly float[] defaultSpawnChancesValues = { 50, 50 };
    private readonly List<State> defaultSkinsState = null;
    private readonly Player defaultSkin = null;
    private readonly bool defaultCanPlay = true;

    public int CurrentPointIndex;
    public float StartBarValue;
    public float EndBarValue;
    public int Score;
    public int Coins;
    public int CurrentZoneIndex;
    public List<float> SpawnChances;
    public List<State> SkinsState;
    public Player CurrentSkin;
    public bool CanPlay;

    public SaveData(int currentPointIndex, float startBarValue, float endBarValue, int score, int coins,
        int currentZoneIndex,
        List<float> spawnChances, List<State> skinsState, Player currentSkin, bool canPlay)
    {
        CurrentPointIndex = currentPointIndex;
        StartBarValue = startBarValue;
        EndBarValue = endBarValue;
        Score = score;
        Coins = coins;
        CurrentZoneIndex = currentZoneIndex;
        SpawnChances = spawnChances;
        SkinsState = skinsState;
        CurrentSkin = currentSkin;
        CanPlay = canPlay;
    }

    public SaveData()
    {
        CurrentPointIndex = defaultCurrentPointIndex;
        StartBarValue = defaultStartBarValue;
        EndBarValue = defaultEndBarValue;
        Score = defaultScore;
        Coins = defaultCoins;
        CurrentZoneIndex = defaultCurrentZoneIndex;
        SpawnChances = defaultSpawnChancesValues.ToList();
        SkinsState = defaultSkinsState;
        CurrentSkin = defaultSkin;
        CanPlay = defaultCanPlay;
    }
}