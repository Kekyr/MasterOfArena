public class LeaderboardPlayer
{
    public LeaderboardPlayer(string id, string avatar, string name, int rank, int score)
    {
        Id = id;
        Avatar = avatar;
        Name = name;
        Rank = rank;
        Score = score;
    }

    public string Id { get; private set; }
    public string Avatar { get; private set; }
    public string Name { get; private set; }
    public int Rank { get; private set; }
    public int Score { get; private set; }
}