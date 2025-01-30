
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCaseHandler_scr : MonoBehaviour
{
    public GameObject bouncerPrefab;
    public Transform[] bouncerPoints = new Transform[8];
    public List<GameObject> bouncerList;
    public GameDataTracker_scr GameData_;
    public int[] bouncerIndexes = new int[8];
    public bool isTesting; // if true will place everthing in its place 

    void Start() {
        GameData_ = GetComponent<GameDataTracker_scr>();
        enabled = isTesting;
    }
    private void OnEnable() {
        ConstructTestCase(bouncerIndexes);        
    }
    void Update() {
        ExecuteTestCase(GameData_.deliveringCount);
    }

    public void ConstructTestCase(int[] _positionInedxes){
        for (int i = 0; i < _positionInedxes.Length; i++)
        {
            if(_positionInedxes[i] > 7){
                Debug.Log("Numbers can be from interval [0,7]");
                return;
            }
            GameObject bouncerInstance = Instantiate(bouncerPrefab, bouncerPoints[_positionInedxes[i]].position, bouncerPrefab.transform.rotation);
            bouncerList.Add(bouncerInstance);
        }
    }
    public void ConstructTestCase(int[]  _positionInedxes, float[] _numbers){
        for (int i = 0; i < _positionInedxes.Length; i++)
        {
            if(_positionInedxes[i] > 7){
                Debug.Log("Numbers can be from interval [0,7]");
                return;
            }
            GameObject bouncerInstance = Instantiate(bouncerPrefab, bouncerPoints[_positionInedxes[i]].position, bouncerPrefab.transform.rotation);
            bouncerInstance.GetComponent<Bouncer_scr>().bounceNumber = _numbers[i];
            bouncerInstance.GetComponent<Bouncer_scr>().bouncerText.text = _numbers[i].ToString();
            bouncerList.Add(bouncerInstance);
        }
    }
    public void ExecuteTestCase(int limitBullet){
        if(GameData_.deliveringCount == limitBullet){
            GameData_.DisplayData();
            Time.timeScale = 0;
        }
    }
    public void ClearTest(){
        for (int i = 0; i < bouncerList.Count; i++)
        {
            Destroy(bouncerList[i]);            
            bouncerList.Remove(bouncerList[i]);
        }
        
    }
}