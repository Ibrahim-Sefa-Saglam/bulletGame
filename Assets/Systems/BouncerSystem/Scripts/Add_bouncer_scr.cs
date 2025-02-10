using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Systems.SaveSystem;
using UnityEngine;

public class Add_bouncer_scr : MonoBehaviour
{
    public GameObject[] BouncerCells = new GameObject[6];    
    public GameObject[] BouncerPointsOnPanel = new GameObject[8];    
    public GameObject bouncerPrefab; 
    public static GameSaveData gameData;
    public List<BouncerData> bouncerData;
    // Start is called before the first frame update
    void Start()
    {
        
        
        gameData = GameSaveData.Instance;
        bouncerData = gameData.BouncerDataList;
        
        foreach (var data in bouncerData)
        {
            GameObject newBouncer = Bouncer.InstantiateBouncer(bouncerPrefab, data);
            foreach (var point in BouncerPointsOnPanel)
            {
                if(newBouncer.transform.position == point.transform.position) point.GetComponent<Collider>().enabled = false;
                 
            }
        }
        
    }
    public void OnClick(){

        for(int i=0; i<BouncerCells.Length;i++){
            Debug.Log("first for i: "+i);
            bool createBouncer = true;
            for(int k=0; k<bouncerData.Count;k++)
            { 
                if(bouncerData[k].bouncerObject.transform.position == BouncerCells[i].transform.position ) createBouncer = false;
                Debug.Log("second for K, createVouncer: "+k+" "+createBouncer);
                
                 
            }

            Debug.Log("createBounce: "+createBouncer);
            if(createBouncer)
            {
                GameObject  newBouncer = Bouncer.InstantiateBouncer(bouncerPrefab, null, BouncerCells[i].transform);
                bouncerData.Add(newBouncer.GetComponent<Bouncer>().BouncerData);
                return;
            }

        }

    }
} 