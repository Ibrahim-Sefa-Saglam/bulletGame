using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;


public class GameData
{
    public List<BouncerData> BouncerData;
    public int PlayerCoinCount;
    public int LevelIndex;

    GameData(int levelIndex)
    {
        LevelIndex = levelIndex;
        
        BouncerData = DataManager.CurrentGameData.BouncerData;
        
        
    }
}

public static class DataManager
    {
           
        public static GameData CurrentGameData;        
        public static void SaveGameData(){} 
        public static void LoadGameData(){}
    }
