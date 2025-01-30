using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBulletInteractable
{
    bool CanInteract{
        get;
        set;
    }
    GameObject InteractableObject{
        get;
        set;
    }
   void InteractBullet(Action callback, GameObject bullet, out bool isDestroy);
}

