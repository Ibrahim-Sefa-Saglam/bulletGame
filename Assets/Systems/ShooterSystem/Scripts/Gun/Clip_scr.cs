using System;
using System.Collections;
using System.Collections.Generic;
using Systems.RunnerSystem;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class Clip_scr : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletGenerationPoint; 
    public GameObject[] clipPositions = new GameObject[10];
    public GameObject gun;
    public GunScr gunScr;
    public List<GameObject> Clip = new();
    public List<float> bulletList = new();
    public List<float> CacheClip = new(); // fill from ClipFiller
    public float rate;
    public float bulletDistance;
    private float TimeElapsed;
    private float TimeElapsedForStarting;
 
 

    void Start()
    {
        gunScr = gun.GetComponent<GunScr>();
        rate = gunScr.rate;
        InitializeClipContent();

        
    }        
    
    // Update is called once per frame
    void Update()
    {
        rate = gunScr.rate;
        TimeElapsedForStarting += Time.deltaTime;
        if(TimeElapsedForStarting > 0.9f) MoveBullets();
        CreateBullet();
        
    }   

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("bullet")){
            float bulletDamage = other.GetComponent<BulletScr>().BulletInfo.Damage;            
            GunScr gunScript = gun.GetComponent<GunScr>();
            gunScript.FireFromClip(bulletDamage);
            
            Clip.Remove(other.gameObject);
            
            Destroy(other.gameObject);
            
        }
    }
        
    // this is used by camera script, once 
    public void InitializeClipContent(){

        bulletDistance = 1;
        Vector3 newPoint = new( transform.position.x , transform.position.y + 1f , GetComponent<Collider>().bounds.min.z + 2f); // GetComponent<Collider>().bounds.min.z - 0.8f
        
        GameObject initialBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
        initialBullet.layer = 3;
        
        BulletScr bulletScr = initialBullet.GetComponent<BulletScr>();
        BulletInfo bulletInfo = new BulletInfo(GetBulletDamageFromList());
        bulletScr.BulletInfo = bulletInfo; 
        bulletInfo.Damage = GetBulletDamageFromList();
        bulletScr.inClip = true;
        
        Clip.Add(initialBullet);
        
        for (int i = 0; i <15; i++) // (int i = 1; i < 11; i++)
        {
            Vector3 point =  new(transform.position.x, transform.position.y + 1f ,Clip[i].transform.position.z + bulletDistance); // clipPositions[i].transform.position;
            GameObject newBullet = Instantiate(bulletPrefab,point,transform.rotation,transform);
            newBullet.layer = 3;
            bulletScr = newBullet.GetComponent<BulletScr>();
            bulletScr.inClip = true;
            bulletScr.BulletInfo.Damage = GetBulletDamageFromList();
            bulletScr.BulletInfo.BulletText = bulletScr.BulletInfo.Damage.ToString();
            bulletScr.numberText.text = bulletScr.BulletInfo.Damage.ToString();
            Clip.Add(newBullet);
        }
    }
    public float GetBulletDamageFromList(){
        if(bulletList.Count > 0){
            float damage =  bulletList[0];
            bulletList.RemoveAt(0);
            return damage;
        }
        else if( CacheClip.Count > 0){
            float damage = CacheClip[(int)UnityEngine.Random.Range(0f, CacheClip.Count)];
            return damage;
        }
        else{
            return Mathf.Floor(UnityEngine.Random.Range(5f,30f));
        }
    } 
    public void MoveBullets()
    {
        // Loop through each bullet in the list
        foreach (GameObject bullet in Clip)
        {
            // Move the bullet to the right (positive X direction) by the specified speed
            bullet.transform.position += transform.right * (rate * Time.deltaTime) ;
        }
    }

    void CreateBullet(){
            TimeElapsed+= Time.deltaTime;
            if(Clip.Count < 15  ){
                for(int i=0; i<15;i++){
                
                
                    Vector3 newPoint =  new(transform.position.x, transform.position.y + 1f ,Clip[^1].transform.position.z  + bulletDistance); // clipPositions[i].transform.position;
                    GameObject newBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
                    BulletScr bulletScr = newBullet.GetComponent<BulletScr>();
                    bulletScr.InitializeForClip(GetBulletDamageFromList());
                    Clip.Add(newBullet);
                }
            }
            else if (TimeElapsed >= 1/rate)
            {
                TimeElapsed = 0;
                Vector3 newPoint =  new(transform.position.x, transform.position.y + 1f ,Clip[^1].transform.position.z  + bulletDistance); // clipPositions[i].transform.position;
                GameObject newBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
                BulletScr bulletScr = newBullet.GetComponent<BulletScr>();
                newBullet.layer = 3;
                bulletScr = newBullet.GetComponent<BulletScr>();
                bulletScr.inClip = true;
                bulletScr.BulletInfo.Damage = GetBulletDamageFromList();
                bulletScr.BulletInfo.BulletText = bulletScr.BulletInfo.Damage.ToString();
                Clip.Add(newBullet);
            }
        
        
}
}
