public static class StaticLevelData
{
    public static LevelDifficulty chosenDifficulty;
    
    public static void SetDifficulty(LevelDifficulty difficulty)
    {
        chosenDifficulty = difficulty;
    }
}

public enum LevelDifficulty
{
    Easy,
    Medium,
    Hard
}