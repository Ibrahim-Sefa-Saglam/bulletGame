using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Cinemachine;
using Systems.SaveSystem;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Camera_scr : MonoBehaviour
{
    public CinemachineVirtualCamera runnerVCamera;
    public CinemachineVirtualCamera bouncerVCamera;
    public GameObject gun; // Reference to the gun object
    public GunScr GunScript;

    void Start()
    {
        ActivateBouncerMode();
        GameStateHandler.OnEnterState += OnEnterStateBehaviours;
    }

    private void OnEnterStateBehaviours(GameStateHandler.GameStates enterState)
    {
        switch (enterState)
        {
            case GameStateHandler.GameStates.Bouncer:
                ActivateBouncerMode();
                break;
        }
    }
    public void ActivateBouncerMode()
    {
        bouncerVCamera.Priority = 10;
        runnerVCamera.Priority = 0;
        GunScript = gun.GetComponent<GunScr>();
        GunScript.enabled = false;
    }
    public void ActivateFollowMode()
    {
        GameStateHandler.SetState(GameStateHandler.GameStates.Runner);
        bouncerVCamera.Priority = 0;
        runnerVCamera.Priority = 10;
        
        GunScript.StartAllGunBehavior();
        
    }
    public void AssignGunToFollow(GameObject newGun)
    {
        gun = newGun;
        GunScript = gun.GetComponent<GunScr>();
        GunScript.camera = this.gameObject;
        runnerVCamera.Follow = gun.transform;

        ActivateBouncerMode();
    }

   
    
}
