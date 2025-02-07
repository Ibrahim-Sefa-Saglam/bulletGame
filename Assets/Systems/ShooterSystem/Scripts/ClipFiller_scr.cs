using System;
using System.Collections;
using System.Collections.Generic;
using Systems.RunnerSystem;
using Unity.VisualScripting;
using UnityEngine;

public class ClipFiller_scr : MonoBehaviour, IBulletInteractable
{

    public GameObject clip;
    private Clip_scr clipScr;

    public bool CanInteract { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public GameObject InteractableObject { get ; set ; }

    void Start()
    {   
        InteractableObject = gameObject;
        clipScr = clip.GetComponent<Clip_scr>();
    }        

    void HandlePanelBulletCollision(IBullet bullet){
        
        float bulletDamage = bullet.BulletInfo.Damage;
    
        clipScr.bulletList.Add(bulletDamage);
        clipScr.CacheClip.Add(bulletDamage);
        
        if(clipScr.CacheClip.Count>10) clipScr.CacheClip.RemoveAt(0);
    
    }

    public void InteractBullet(Action callback, IBullet bullet, out bool isDestroy)
    {   
        isDestroy = false;
        if(bullet  == null) return;
        HandlePanelBulletCollision(bullet);
        isDestroy = true;
    }
}
