using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Systems.SaveSystem
{
    [Serializable]
    public class GameSaveData : SaveData
    {
        public int levelIndex;
        public int coinScore;
        [SerializeField]
        public List<BouncerData> BouncerDataList = new List<BouncerData>();
        
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

                string defaultJson = JsonUtility.ToJson(this);
                File.WriteAllText(Application.persistentDataPath + "/GameSaveData.json", defaultJson);
            }
            else
            {
                var file = JsonUtility.ToJson(this);
                File.WriteAllText(Application.persistentDataPath + "/GameSaveData.json", file);
            }
        }

        public override SaveData Load()
        {
            Debug.Log(Application.persistentDataPath + "/GameSaveData.json");
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
            BouncerDataList = gameSaveData.BouncerDataList;
            
            return gameSaveData;
        }
 
    }
}