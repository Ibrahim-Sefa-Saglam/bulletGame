using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipFiller_scr : MonoBehaviour
{

    public GameObject clip;
    public GameObject dataTrackerObject;             // Bullet prefab to instantiate
    public GameDataTracker dataTracker;
    private Clip_scr clipScr;
    // Start is called before the first frame update
    void Start()
    {
        clipScr = clip.GetComponent<Clip_scr>();
        dataTracker = dataTrackerObject.GetComponent<GameDataTracker>();
    }        
    private void OnCollisionEnter(Collision other) {        
        if(other.gameObject.CompareTag("bullet")){
            PanelBullet_scr panelBullet_Scr = other.gameObject.GetComponent<PanelBullet_scr>();
            dataTracker.AddBulletDamage(panelBullet_Scr.damage);
            dataTracker.AddPanelTime(panelBullet_Scr.panelTime);
            dataTracker.AddDeliveringRate(1);

            float bulletDamage = panelBullet_Scr.damage;
            if(clipScr.bulletList.Count<100){clipScr.bulletList.Add(bulletDamage);}            
            Destroy(other.gameObject);
        }
    }
}
