using System;
using UnityEngine;

public static class GameStateHandler
{
    [SerializeField]
    private static GameStates _currentState = GameStates.Menu;
    public enum GameStates
    {
        Menu = 0,
        Lose = 1,
        Win = 2,
        Runner = 3,
        Bouncer = 4,
        Empty = 5
    }

    public static event Action<GameStates> OnExitState;
    public static event Action<GameStates> OnEnterState;
    
    public static void Init()
    {
        
    }
    public static void SetState(GameStates state)
    {
        FireOnExitState(_currentState);
        _currentState = state;
        FireOnEnterState(_currentState);
    }

    private static void FireOnEnterState(GameStates state)
    {
        OnEnterState?.Invoke(state);
    }

    private static void FireOnExitState(GameStates state)
    {
        OnExitState?.Invoke(state);
    }
}