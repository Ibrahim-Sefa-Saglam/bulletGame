using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerInteractable
{
    bool CanInteract{
        get;
        set;
    }
   void InteractPlayer(GameObject player);
}

