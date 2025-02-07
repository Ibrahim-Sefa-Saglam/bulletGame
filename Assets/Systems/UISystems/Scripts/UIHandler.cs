using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIHandler: MonoBehaviour 
{
    
    public GameObject gameOverCanvas;
    public GameObject winCanvas;
    public GameObject menuCanvas;
    public GameObject inGameCanvas;
    private GameObject[] _uıCanvases;
    public TextMeshProUGUI inGameCanvasCoinText;
    public GameObject SettingsPanel;
    
    
    void Awake()
    {   
        GameManager.SetUIHandler(this);  
        _uıCanvases = new [] {gameOverCanvas, winCanvas,menuCanvas,inGameCanvas};
        SetCanvas(menuCanvas);
        SettingsPanel.SetActive(false);
        GameStateHandler.OnEnterState += OnEnterStateBehaviours;

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
                ActivateInGameUI();
                break;
            case GameStateHandler.GameStates.Bouncer:
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
        foreach (GameObject _canvas in _uıCanvases)
        {
            if(_canvas != targetCanvas) _canvas.SetActive(false);
        }
        targetCanvas.SetActive(true);
    }

    public void  UpdateUICoinNumber(int _coinNumber)
    {
        inGameCanvasCoinText.text = ": " + _coinNumber.ToString();
    }

    public void SettigsButtonAction() {SettingsPanel.SetActive(!SettingsPanel.activeSelf); }
    public void SettingsResetButtonAction(){ GameManager.ResetGame();}
    public void SettingsWinButtonAction() { GameStateHandler.SetState(GameStateHandler.GameStates.Win);}
    public void SettingsLoseButtonAction() { GameStateHandler.SetState(GameStateHandler.GameStates.Lose);}
}

