using UnityEngine;

/// <summary>
/// Handles game states
/// </summary>
public class StateMachine : MonoBehaviour
{
    public enum GameStates { MainMenu, PlayerShop, PlayerInventory, PauseMenu, Combat, Decorating }
    public enum OpenUIStates { OpenUI, ClosedUI }
    public enum PausedStates { Paused, Unpaused }

    private GameStates currentState;
    private GameStates previousState;
    private OpenUIStates UIState;
    private PausedStates pausedState;

    public GameStates GetCurrentState()
    {
        return currentState;
    }

    public GameStates GetPreviousState()
    {
        return previousState;
    }

    public PausedStates GetPausedState()
    {
        return pausedState;
    }

    public void SetStateMainMenu()
    {
        previousState = currentState;
        currentState = GameStates.MainMenu;
    }
    public void SetStatePlayerShop()
    {
        previousState = currentState;
        currentState = GameStates.PlayerShop;
    }
    public void SetStatePlayerInventory()
    {
        SetStateOpenUI();
        previousState = currentState;
        currentState = GameStates.PlayerInventory;
    }
    public void SetStatePauseMenu()
    {
        SetStateOpenUI();
        previousState = currentState;
        currentState = GameStates.PauseMenu;
    }
    public void SetStateCombat()
    {
        previousState = currentState;
        currentState = GameStates.Combat;
    }
    public void SetStateDecorating()
    {
        previousState = currentState;
        currentState = GameStates.Decorating;
    }
    /// <summary>
    /// This changes the current state to the previous, as well as sets the UI state to closed
    /// </summary>
    public void SetStateToPrevious()
    {
        SetStateClosedUI();
        currentState = previousState;
    }
    private void SetStateOpenUI()
    {
        UIState = OpenUIStates.OpenUI;
    }
    private void SetStateClosedUI()
    {
        UIState = OpenUIStates.ClosedUI;
    }
    public void SetStatePaused()
    {
        pausedState = PausedStates.Paused;
    }
    public void SetStateUnpaused()
    {
        pausedState = PausedStates.Unpaused;
    }
}
