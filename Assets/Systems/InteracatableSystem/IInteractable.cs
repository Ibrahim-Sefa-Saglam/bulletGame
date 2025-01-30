using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    bool CanInteract{
        get;
        set;
    }
   void Interact(Action callback, GameObject passiveObject, out bool isDestroy);
}

