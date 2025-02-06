using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class BouncerData
{
    public static int BouncerCount = 0;
    private int _bouncerNumber;
    private Transform _bouncerTransform;
    
    public BouncerData (int bouncerNumber, Transform bouncerTransform)
    {
            _bouncerNumber = bouncerNumber;
            _bouncerTransform = bouncerTransform;
            BouncerCount++;
    }
    
}

public class GameData
{
    public BouncerData BouncerData;
    public int PlayerCoinCount;
    public int LevelIndex;
}

public static class GameDataManager
    {
        
        static GameDataManager(){}
        
        public static void ConstructNewGameData(){}
        static void SaveGameData(){}
        static void LoadGameData(){}
    }
