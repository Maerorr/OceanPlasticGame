using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadGame
{
    void SaveGame(GameData gameData);
    void LoadGame(ref GameData gameData);
}
