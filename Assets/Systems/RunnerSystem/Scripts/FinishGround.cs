using UnityEngine;

public class FinishGround : MonoBehaviour, IPlayerInteractable
{
    private IPlayerInteractable _playerInteractableImplementation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractPlayer(GameObject player)
    {
        GameStateHandler.SetState( GameStateHandler.GameStates.Win);
    }
}
