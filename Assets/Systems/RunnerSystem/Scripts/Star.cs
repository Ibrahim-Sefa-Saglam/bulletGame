using UnityEngine;

public class Star : MonoBehaviour, IPlayerInteractable
{
    public ParticleSystem starParticle;
    void Start()
    {
        
    }

    public void InteractPlayer(GameObject player)
    {
        GameManager.IncreaseXp(1);
        Destroy(gameObject);
        starParticle.Play();
        var Main = starParticle.main;
        Main.stopAction = ParticleSystemStopAction.Destroy;
    }
}
