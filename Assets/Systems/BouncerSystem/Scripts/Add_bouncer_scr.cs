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
    public int CoinScore
    {
        get => gameData.coinScore;
        set => gameData.coinScore = value;
    }

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

        if(CoinScore < 50) return;
        CoinScore-=50;
        
        GameManager.UIHandler.UpdateUICoinNumber(CoinScore);
        
        for(int i=0; i<BouncerCells.Length;i++){
            bool createBouncer = true;
            for(int k=0; k<bouncerData.Count;k++)
            { 
                if(bouncerData[k].bouncerObject.transform.position == BouncerCells[i].transform.position ) createBouncer = false;
                
                 
            }

            if(createBouncer)
            {
                GameObject  newBouncer = Bouncer.InstantiateBouncer(bouncerPrefab, null, BouncerCells[i].transform);
                bouncerData.Add(newBouncer.GetComponent<Bouncer>().BouncerData);
                return;
            }

        }

    }
} 