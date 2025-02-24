using Systems.SaveSystem;
using UnityEngine;

public static class GameManager  // Manages Key Object generation and camera orientation
{

    public static GameObject Gun;
    public static UIHandler UIHandler;
    private static GameSaveData _gameSaveData;
    
    
    static GameManager()
    {
        GameStateHandler.OnEnterState += OnEnterStateBehaviorGameManager;
        LoadData();
        _gameSaveData = GameSaveData.Instance;
    }

    public static void OnEnterStateBehaviorGameManager(GameStateHandler.GameStates enterState)
    {
        if (enterState.ToString() == GameStateHandler.GameStates.Bouncer.ToString() ||
            enterState.ToString() == GameStateHandler.GameStates.Runner.ToString()) Time.timeScale = 1;
        else Time.timeScale = 0;
        if (enterState.ToString() == GameStateHandler.GameStates.Win.ToString() ||
            enterState.ToString() == GameStateHandler.GameStates.Lose.ToString())
        {
            GameSaveData.Instance.Save();
        } 
    }
    public static void SetUIHandler(UIHandler handler){ UIHandler = handler; }
    public static void SetGun(GameObject gun){ Gun = gun; }
    public static void ResetGame()
    {
        LevelManager.ResetToFirstLevel();
        _gameSaveData.bouncerDataList.Clear();
        _gameSaveData.coinScore = 0;
        _gameSaveData.levelIndex = 0;
        _gameSaveData.exp = 0f;
        _gameSaveData.bulletLevel = 1;
        _gameSaveData.Save();
        UIHandler.UpdateUICoinNumber(0);
    }
    public static void LoadData()
    {
        GameSaveData.Instance.Load();
    }

    public static void IncreaseXp(float amount)
    {
        _gameSaveData.exp += amount;
        if (_gameSaveData.exp >= _gameSaveData.bulletLevel * 20)
        {
            _gameSaveData.bulletLevel++;
            _gameSaveData.exp = 0;
        }
        UIHandler.coloredLevelBar.fillAmount = _gameSaveData.exp / (_gameSaveData.bulletLevel * 20);
        UIHandler.LevelBarAnimation();
    }
}
 