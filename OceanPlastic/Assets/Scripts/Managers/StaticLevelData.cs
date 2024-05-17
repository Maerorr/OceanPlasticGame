using System.Collections.Generic;

public static class StaticLevelData
{
    public static LevelDifficulty chosenDifficulty = LevelDifficulty.Easy;
    public static List<(FloatingTrashSO, int)> collectionObjective;
    
    public static void SetDifficulty(LevelDifficulty difficulty)
    {
        chosenDifficulty = difficulty;
    }
    
    public static void SetCollectionObjective(FloatingTrashSO trash, int amount)
    {
        collectionObjective.Add((trash, amount));
    }
}

public enum LevelDifficulty
{
    Easy = 35,
    Medium = 75,
    Hard = 150
}