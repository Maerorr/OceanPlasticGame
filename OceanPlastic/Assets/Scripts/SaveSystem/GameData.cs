using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int playerMoney;
    public List<CurrentTool> unlockedTools = new List<CurrentTool>();

    public GameData()
    {
        playerMoney = 0;
        unlockedTools.Add(CurrentTool.Vacuum);
    }
}
