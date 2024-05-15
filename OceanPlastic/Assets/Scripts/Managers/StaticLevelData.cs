public static class StaticLevelData
{
    public static LevelDifficulty chosenDifficulty = LevelDifficulty.Easy;
    
    public static void SetDifficulty(LevelDifficulty difficulty)
    {
        chosenDifficulty = difficulty;
    }
}

public enum LevelDifficulty
{
    Easy = 35,
    Medium = 75,
    Hard = 150
}