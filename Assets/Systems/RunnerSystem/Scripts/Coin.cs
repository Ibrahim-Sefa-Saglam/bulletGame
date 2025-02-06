 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IPlayerInteractable
{
    public ParticleSystem CoinParticle;
    public Vector3 coinStartPosition;
    public float hoverAmplitude = 0.5f; // The maximum height difference of the hover
    public float hoverFrequency = 5f;
    void Start()
    {
        coinStartPosition = transform.position;
        
    }
    void Update()
    {
        CoinHover();         
    }
    public bool CanInteract { get; set; }
    public void InteractPlayer(GameObject player)
    {
        GameManager.IncrementSavedCoinCount(1); // increment the PlayerPrefs.CoinCount by 1
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
}   
