using System.Collections.Generic;

public static class StaticLevelData
{
    public static LevelDifficulty chosenDifficulty = LevelDifficulty.Easy;
    public static List<FloatingTrashSO> collectionObjective;
    public static bool isInLevel = false;
    
    public static void SetDifficulty(LevelDifficulty difficulty)
    {
        chosenDifficulty = difficulty;
    }
    
    public static void AddCollectionObjective(FloatingTrashSO trash)
    {
        collectionObjective.Add(trash);
    }
}

public enum LevelDifficulty
{
    Easy = 35,
    Medium = 75,
    Hard = 150
}