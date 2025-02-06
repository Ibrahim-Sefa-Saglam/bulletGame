using System;
using TMPro;
using UnityEngine;

public class UIHandler: MonoBehaviour 
{
    
    public Canvas gameOverCanvas;
    public Canvas winCanvas;
    public Canvas menuCanvas;
    public Canvas inGameCanvas;
    private Canvas[] _uıCanvases;
    public TextMeshProUGUI inGameCanvasCoinText;
    
    
    void Awake()
    {   
        GameManager.SetUIHandler(this);  
        _uıCanvases = new [] {gameOverCanvas, winCanvas,menuCanvas,inGameCanvas};
        SetCanvas(menuCanvas);

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
        
        LevelManager.GenerateNextLevel();
        // CHANGE THE REST OF THIS METHOD AS FOLLOWS:  
        // Handle runner generation in GameManager.cs 
        
      
    }
    public void SetCanvas(Canvas targetCanvas)
    {
        foreach (Canvas _canvas in _uıCanvases)
        {
            Debug.Log("canvasLength"+_uıCanvases.Length);
         
            Debug.Log(gameOverCanvas.name );
            
            if(_canvas != targetCanvas) _canvas.enabled = false;
             
        }
        targetCanvas.enabled = true;
    }

    public void  UpdateUICoinNumber(int _coinNumber)
    {
        inGameCanvasCoinText.text = ": " + _coinNumber.ToString();
    }

    public void SettigsButtonAction()
    {
     GameManager.ResetGame();
     _uıCanvases = new [] {gameOverCanvas, winCanvas,menuCanvas,inGameCanvas};
    }
}

