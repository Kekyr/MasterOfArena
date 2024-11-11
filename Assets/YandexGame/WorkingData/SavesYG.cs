using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int CurrentPointIndex = 0;
        public float StartBarValue = 0;
        public float EndBarValue = 0.2f;
        public int Score = 0;
        public int Coins = 0;
        public int CurrentZoneIndex = 0;
        public List<float> SpawnChances = new List<float>(new float[] { 50, 50 });
        public int[] CubesIndex = { 0, 1, 0 };
        public List<State> SkinsState = null;
        public int CurrentSkinIndex = 0;
        public bool CanPlay = true;
        public bool IsMusicOn = true;
        public int MusicSpriteIndex = 0;
        public bool IsSFXOn = true;
        public int SFXSpriteIndex = 0;

        public SavesYG(int currentPointIndex, float startBarValue, float endBarValue, int score, int coins,
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

        public SavesYG()
        {
        }
    }
}