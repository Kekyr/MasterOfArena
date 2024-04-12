public class LeaderboardPlayer
{
    public LeaderboardPlayer(string avatar, string name, int rank, int score)
    {
        Avatar = avatar;
        Name = name;
        Rank = rank;
        Score = score;
    }

    public string Avatar { get; private set; }
    public string Name { get; private set; }
    public int Rank { get; private set; }
    public int Score { get; private set; }
}