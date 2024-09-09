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

    private readonly float[] defaultSpawnChancesValue = { 50, 50 };
    private readonly int[] defaultCubesIndex = { 0, 1, 0 };
    private readonly List<State> defaultSkinsState = null;
    private readonly int defaultSkinIndex = 0;
    private readonly bool defaultCanPlay = true;
    private readonly bool defaultIsMusicOn = true;
    private readonly int defaultMusicSpriteIndex = 0;
    private readonly bool defaultIsSFXOn = true;
    private readonly int defaultSFXSpriteIndex = 0;

    public int CurrentPointIndex;
    public float StartBarValue;
    public float EndBarValue;
    public int Score;
    public int Coins;
    public int CurrentZoneIndex;
    public List<float> SpawnChances;
    public int[] CubesIndex;
    public List<State> SkinsState;
    public int CurrentSkinIndex;
    public bool CanPlay;
    public bool IsMusicOn;
    public int MusicSpriteIndex;
    public bool IsSFXOn;
    public int SFXSpriteIndex;

    public SaveData(int currentPointIndex, float startBarValue, float endBarValue, int score, int coins,
        int currentZoneIndex,
        List<float> spawnChances, int[] cubesIndex, List<State> skinsState, int currentSkinIndex, bool canPlay,
        bool isMusicOn, int musicSpriteIndex, bool isSFXOn, int sfxSpriteIndex)
    {
        CurrentPointIndex = currentPointIndex;
        StartBarValue = startBarValue;
        EndBarValue = endBarValue;
        Score = score;
        Coins = coins;
        CurrentZoneIndex = currentZoneIndex;
        SpawnChances = spawnChances;
        CubesIndex = cubesIndex;
        SkinsState = skinsState;
        CurrentSkinIndex = currentSkinIndex;
        CanPlay = canPlay;
        IsMusicOn = isMusicOn;
        MusicSpriteIndex = musicSpriteIndex;
        IsSFXOn = isSFXOn;
        SFXSpriteIndex = sfxSpriteIndex;
    }

    public SaveData()
    {
        CurrentPointIndex = defaultCurrentPointIndex;
        StartBarValue = defaultStartBarValue;
        EndBarValue = defaultEndBarValue;
        Score = defaultScore;
        Coins = defaultCoins;
        CurrentZoneIndex = defaultCurrentZoneIndex;
        SpawnChances = defaultSpawnChancesValue.ToList();
        CubesIndex = defaultCubesIndex;
        SkinsState = defaultSkinsState;
        CurrentSkinIndex = defaultSkinIndex;
        CanPlay = defaultCanPlay;
        IsMusicOn = defaultIsMusicOn;
        MusicSpriteIndex = defaultMusicSpriteIndex;
        IsSFXOn = defaultIsSFXOn;
        SFXSpriteIndex = defaultSFXSpriteIndex;
    }
}