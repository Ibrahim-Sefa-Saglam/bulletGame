using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager  // Manages Key Object generation and camera orientation
{   
    
    [SerializeField]
    private static string gameState;
    private static UIHandler _uiHandler;
    public enum PlayerPrefsNames
    {
        CoinCount = 0,
        Level = 1,
    }
    
    static GameManager()
    {
        GameStateHandler.OnEnterState += OnEnterStateBehaviorGameManager;
        
        PlayerPrefs.SetInt(PlayerPrefsNames.CoinCount.ToString(), 0);

    }

    public static void OnEnterStateBehaviorGameManager(GameStateHandler.GameStates enterState)
    {
        gameState = enterState.ToString();
        if (gameState == GameStateHandler.GameStates.Bouncer.ToString() ||
            gameState == GameStateHandler.GameStates.Runner.ToString()) Time.timeScale = 1;
        else Time.timeScale = 0;
    }
    public static void SetUIHandler(UIHandler handler){ _uiHandler = handler; }
    public static void IncrementSavedCoinCount(int coinCount)
    {
        Debug.Log(PlayerPrefsNames.CoinCount.ToString()+" : "+ PlayerPrefs.GetInt(PlayerPrefsNames.CoinCount.ToString(), -1) );
        
        PlayerPrefs.GetInt(PlayerPrefsNames.CoinCount.ToString(), -1) ;
        int newCoinCount = PlayerPrefs.GetInt(PlayerPrefsNames.CoinCount.ToString(), 0) + coinCount;

        Debug.Log("newCount: " + newCoinCount.ToString());
        _uiHandler.UpdateUICoinNumber(newCoinCount);
        PlayerPrefs.SetInt(PlayerPrefsNames.CoinCount.ToString(), newCoinCount);
        
        Debug.Log(PlayerPrefsNames.CoinCount.ToString()+" : "+ PlayerPrefs.GetInt(PlayerPrefsNames.CoinCount.ToString(), -1) );
        
        PlayerPrefs.Save();
    }

    public static void ResetGame()
    {
        PlayerPrefs.SetInt(PlayerPrefsNames.CoinCount.ToString(), 0);
        PlayerPrefs.SetInt(PlayerPrefsNames.Level.ToString(), 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}

