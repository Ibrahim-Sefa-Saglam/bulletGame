using System;
using Systems.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIHandler: MonoBehaviour 
{
    
    public GameObject gameOverCanvas;
    public GameObject winCanvas;
    public GameObject menuCanvas;
    public GameObject inGameCanvas;
    private GameObject[] _uıCanvases;
    public GameObject settingsPanel;
    public TextMeshProUGUI inGameCanvasCoinText;
    public GameObject levelPanel;
    public Image coloredLevelBar;
    void Awake()
    {   
        GameManager.SetUIHandler(this);  
        GameStateHandler.OnEnterState += OnEnterStateBehaviours;
        _uıCanvases = new [] {gameOverCanvas, winCanvas,menuCanvas,inGameCanvas};
        SetCanvas(menuCanvas);
        settingsPanel.SetActive(false);
        inGameCanvasCoinText.text = GameSaveData.Instance.coinScore.ToString();
        coloredLevelBar.fillAmount = GameSaveData.Instance.exp / (GameSaveData.Instance.bulletLevel * 20);
    }

    private void OnEnterStateBehaviours(GameStateHandler.GameStates enterState)
    {
        switch (enterState)
        {
            case GameStateHandler.GameStates.Menu:
                GoToMenuUI();
                break;
            case GameStateHandler.GameStates.Lose:
                ActivateGameOverUI();
                break;
            case GameStateHandler.GameStates.Win:
                ActivateWinUI();
                break;
            case GameStateHandler.GameStates.Runner:
                levelPanel.SetActive(true);
                ActivateInGameUI();
                break;
            case GameStateHandler.GameStates.Bouncer:
                levelPanel.SetActive(false);
                ActivateInGameUI();
                break;
            case GameStateHandler.GameStates.Empty:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(enterState), enterState, null);
        }
    }

   
    void ActivateInGameUI()
    {
        SetCanvas(inGameCanvas);
    }
    void ActivateGameOverUI()
    {
        SetCanvas(gameOverCanvas);
    }
    void ActivateWinUI()
    {
        SetCanvas(winCanvas);
    }
    public void GoToMenuUI() // is by UI buttons 
    {
        GameStateHandler.SetState(GameStateHandler.GameStates.Menu);
        SetCanvas(menuCanvas);
    }
    public void PlayFromMenuUI()
    {
        GameStateHandler.SetState(GameStateHandler.GameStates.Bouncer);
    }
    public void RetryGame() // used by UI WimPanel's and LosePanel's buttons
    {
        GameStateHandler.SetState(GameStateHandler.GameStates.Bouncer);
        LevelManager.GenerateCurrentLevel();
    }
    public void NextLevel()// used in UI
    {
        GameStateHandler.SetState(GameStateHandler.GameStates.Bouncer);
        SetCanvas(inGameCanvas);
        
        LevelManager.LevelUp();
        // CHANGE THE REST OF THIS METHOD AS FOLLOWS:  
        // Handle runner generation in GameManager.cs 
        
      
    }
    public void SetCanvas(GameObject targetCanvas)
    {
        foreach (GameObject canvas in _uıCanvases)
        {
            if(canvas != targetCanvas) canvas.SetActive(false);
        }
        targetCanvas.SetActive(true);
    }
    public void  UpdateUICoinNumber(int coinNumber)
    {
        inGameCanvasCoinText.text = ": " + coinNumber.ToString();
    }
    public void SettigsButtonAction() {settingsPanel.SetActive(!settingsPanel.activeSelf); }
    public void SettingsResetButtonAction(){ GameManager.ResetGame();}
    public void SettingsWinButtonAction() { GameStateHandler.SetState(GameStateHandler.GameStates.Win);}
    public void SettingsLoseButtonAction() { GameStateHandler.SetState(GameStateHandler.GameStates.Lose);}
    public void SettingsCoinButtonAction()
    {
        GameSaveData.Instance.coinScore += 50;
        UpdateUICoinNumber(GameSaveData.Instance.coinScore);
    }
}

