using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.SaveSystem
{
    [Serializable]
    public class GameSaveData : SaveData
    {
        public int levelIndex;
        public int coinScore;
        public float exp;
        public int bulletLevel;
        [SerializeField]
        public List<BouncerData> bouncerDataList = new List<BouncerData>();
        
        
        private static GameSaveData _instance;

        public static GameSaveData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameSaveData();
                }
                return _instance;
            }
        }

        private GameSaveData()
        {
            
        }
        
        public override void Save()
        {
            if (!File.Exists(Application.persistentDataPath + "/GameSaveData.json"))
            {
                Debug.LogWarning("Save file not found! Creating a new one with default values.");
                
                levelIndex = 0;
                coinScore = 0;
                exp  = 0;
                bulletLevel  =1 ;       
                string defaultJson = JsonUtility.ToJson(this,true);
                File.WriteAllText(Application.persistentDataPath + "/GameSaveData.json", defaultJson);
            }
            else
            {
                foreach (var bouncerData in bouncerDataList)
                {
                    bouncerData.SerializeData();
                }

                var file = JsonUtility.ToJson(this,true);
                File.WriteAllText(Application.persistentDataPath + "/GameSaveData.json", file);
            }
        }

        public override SaveData Load()
        {
            if (!File.Exists(Application.persistentDataPath + "/GameSaveData.json"))
            {
                Debug.LogWarning("Save file not found! Creating a new one.");
                Save(); // Save a new default file
                return this; // Return the new instance
            }
            var json = File.ReadAllText(Application.persistentDataPath + "/GameSaveData.json");
            var gameSaveData = JsonUtility.FromJson<GameSaveData>(json);
            levelIndex = gameSaveData.levelIndex;
            coinScore = gameSaveData.coinScore;
            bouncerDataList = gameSaveData.bouncerDataList;
            exp = gameSaveData.exp;
            bulletLevel = gameSaveData.bulletLevel;
            
            return gameSaveData;
        }
 
    }
}