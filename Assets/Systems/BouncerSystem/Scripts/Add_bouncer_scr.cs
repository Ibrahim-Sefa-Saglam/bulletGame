using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Systems.SaveSystem;
using UnityEngine;

public class Add_bouncer_scr : MonoBehaviour
{
    public GameObject[] BouncerCells = new GameObject[6];    
    public GameObject bouncerPrefab; 
    public static GameSaveData gameData = GameSaveData.Instance;
    public List<BouncerData> bouncerData;
    // Start is called before the first frame update
    void Start()
    {
        bouncerData = gameData.BouncerDataList;
        Debug.Log(bouncerData.Count);
        for(int i=0; i<bouncerData.Count;i++){
            Bouncer.InstantiateBouncer(bouncerPrefab, bouncerData[i]);
        }
    }
    public void OnClick(){

        for(int i=0; i<BouncerCells.Length;i++){
            bool createBouncer = true;
            for(int k=0; k<bouncerData.Count;k++)
            { 
                if(bouncerData[k].bouncerPosition == BouncerCells[i].transform.position) createBouncer = false;
            }
            if(createBouncer)
            {
                Bouncer.InstantiateBouncer(bouncerPrefab, null, BouncerCells[i].transform);
                return;
            }

        }

    }
} 