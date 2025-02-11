using System;
using Systems.RunnerSystem;
using Systems.SaveSystem;
using TMPro;
using UnityEngine;

public class FloatingGate : MonoBehaviour, IBulletInteractable, IPlayerInteractable
{
    public GameObject InteractableObject { get => gameObject; set { } }
    public TextMeshProUGUI text;
    public ParticleSystem particle;
    public int coinPoints = 1;
    public float moveDistance = 4f; // Distance to move left and right
    public float moveSpeed = 7f;    // Speed of movement
    private float _startZ;            // Initial position

    void Start()
    {
        _startZ = transform.position.z; // Store the initial X position
        text.text = "+"+coinPoints.ToString();
    }

    void Update()
    {
        float newZ = _startZ + Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    public void InteractBullet(Action callback, IBullet bullet, out bool isDestroy)
    {
        isDestroy = true;
        coinPoints++;
        text.text = "+"+coinPoints.ToString();
        callback?.Invoke();
    }

    public void InteractPlayer(GameObject player)
    {
        GameSaveData.Instance.coinScore += coinPoints;
        GameManager.UIHandler.UpdateUICoinNumber(GameSaveData.Instance.coinScore);
        particle.Play();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        Destroy(gameObject);
    }
}