using System;
using System.Collections;
using System.Collections.Generic;
using Systems.RunnerSystem;
using Unity.VisualScripting;
using UnityEngine;

public interface IBulletInteractable
{
 
    GameObject InteractableObject{
        get;
        set;
    }
    
    
    
   void InteractBullet(Action callback, IBullet bullet, out bool isDestroy);
}

