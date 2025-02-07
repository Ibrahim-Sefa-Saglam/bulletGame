using Systems.SaveSystem;
using UnityEngine;

public class Coin : MonoBehaviour, IPlayerInteractable
{
    public ParticleSystem CoinParticle;
    public Vector3 coinStartPosition;
    public float hoverAmplitude = 0.5f; // The maximum height difference of the hover
    public float hoverFrequency = 5f;
    private GameSaveData _gameData = GameSaveData.Instance;

    private void Start()
    {
        coinStartPosition = transform.position;
    }

    private void Update()
    {
        CoinHover();         
    }
    public bool CanInteract { get; set; }
    public void InteractPlayer(GameObject player)
    {
        IncreasePlayerCoins(1);  // increase
        CreateCoinParticleEffect();
    }
    public void CreateCoinParticleEffect(){
        ParticleSystem coinParticleInstance = Instantiate(CoinParticle,transform.position,transform.rotation);
        coinParticleInstance.Play();
        Destroy(coinParticleInstance,.3f);
        
        DestroySelf();
    }    
    public void CoinHover ()
    {
            // Calculate the new position
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;

        // Apply the new position while maintaining original X and Z
        transform.position = coinStartPosition + new Vector3(0f, hoverOffset, 0f);
        
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void IncreasePlayerCoins(int addedCoins){ 
        
        _gameData.coinScore += addedCoins;
        GameManager.UIHandler.UpdateUICoinNumber(_gameData.coinScore);
    }
}   
