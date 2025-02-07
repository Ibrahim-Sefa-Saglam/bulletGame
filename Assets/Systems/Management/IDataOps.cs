using System.Collections.Generic;

namespace Systems.Management
{
    public interface IDataOps
    {
                
        public static GameData CurrentGameData { get; set; } = DataManager.CurrentGameData;

        public void LoadData();

        public static void UpdateCoinData(int newCoinCount)
        {
            CurrentGameData.PlayerCoinCount = newCoinCount;
        }
        public static void UpdateLevelData(int newLevelIndex)
        {
            CurrentGameData.LevelIndex = newLevelIndex;
        }
       
    }
}