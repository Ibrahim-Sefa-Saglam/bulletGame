using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData_scr : MonoBehaviour
{
    public GameObject gun;
    public GameObject clip;
    public float FireRate;
    public float MaxFireRate;
    public float MinFireRate;
    public float BulletLife;
    void Start()
    {
        FireRate = 1.5f;
        MinFireRate = 0.2f;
        MinFireRate = 20f;
        BulletLife = 3;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
