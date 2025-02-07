using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Add_bouncer_scr : MonoBehaviour
{
    public GameObject[] BouncerCells = new GameObject[6];    
    public GameObject bouncerPrefab; 
    public GameObject[] bouncers;
    // Start is called before the first frame update
    void Start()
    {
        bouncers = new GameObject[6]; // Create a new array of size 6
        for (int i = 0; i < bouncers.Length; i++)
        {
            bouncers[i] = null; // Explicitly set each element to null (optional)
        }
    }
    public void OnClick(){

        for(int i=0; i<6;i++){
            if(bouncers[i] == null){
                bouncers[i] = Instantiate(bouncerPrefab,BouncerCells[i].transform.position,BouncerCells[i].transform.rotation);
            }
        }

    }
} 