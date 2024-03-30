using UnityEngine;

public class LeaderboardPlayer
{
    public LeaderboardPlayer(Sprite avatar, string name, int rank, int score)
    {
        Avatar = avatar;
        Name = name;
        Rank = rank;
        Score = score;
    }

    public Sprite Avatar { get; private set; }
    public string Name { get; private set; }
    public int Rank { get; private set; }
    public int Score { get; private set; }
}