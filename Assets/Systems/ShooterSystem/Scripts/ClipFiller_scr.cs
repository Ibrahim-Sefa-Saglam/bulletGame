using System;
using System.Collections.Generic;
using Systems.RunnerSystem;
using UnityEngine;

public class ClipFiller : MonoBehaviour, IBulletInteractable
{

    public GameObject clip;
    private List<float> bulletDamageList;
    private ClipScript _clipScr;

    public GameObject InteractableObject { get ; set ; }

    void Start()
    {   
        InteractableObject = gameObject;
    }        

    void HandlePanelBulletCollision(IBullet bullet){
        
        
        float bulletDamage = bullet.BulletInfo.Damage;
        
        if(clip == null)
        {
            if (GameManager.Gun != null)
            {
                clip = GameManager.Gun.GetComponent<GunScr>().clip;
                _clipScr = clip.GetComponent<ClipScript>();
            }
            else
            {
                bulletDamageList.Add(bulletDamage);
                if(bulletDamageList.Count>10) bulletDamageList.RemoveAt(0);
            }
        }
        else  
        {
            _clipScr.bulletList.Add(bulletDamage);
            _clipScr.cacheClip.Add(bulletDamage);
            if(_clipScr.cacheClip.Count>10) _clipScr.cacheClip.RemoveAt(0);
        }
        
        
    
    }

    public void InteractBullet(Action callback, IBullet bullet, out bool isDestroy)
    {   
        isDestroy = true;
        if(bullet  == null) return;
        HandlePanelBulletCollision(bullet);
        isDestroy = true;
    }
}
