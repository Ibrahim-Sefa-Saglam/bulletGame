using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UIElements;

public class Clip_scr : MonoBehaviour
{
    public GameObject gun;
    public GameObject bulletPrefab;
    public GameObject bulletGenerationPoint; 
    public List<GameObject> Clip = new();
    public List<float> bulletList = new();
    public List<float> CacheClip = new(); // fill from ClipFiller
    public GameObject[] clipPositions = new GameObject[10];
    public float rate;
    public float bulletDistance;
    private float TimeElapsed;


    void Start()
    {
        rate = gun.GetComponent<GunScr>().rate;
    }        

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("bullet")){
            float bulletDamage = other.GetComponent<Bullet_scr>().damage;            
            GunScr gunScript = gun.GetComponent<GunScr>();
            gunScript.FireFromClip(bulletDamage);
            
            Clip.Remove(other.gameObject);
            
            Destroy(other.gameObject);
            
        }
    }
    void OnEnable() {
        TimeElapsed = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        MoveBullets();
    }   
    public void InitializeClipBullets(){

        bulletDistance = 1;
        Vector3 newPoint = clipPositions[0].transform.position;// new(GetComponent<Collider>().bounds.min.x - 0.8f, transform.position.y + 0.5f ,transform.position.z);
        GameObject InitialBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
        InitialBullet.layer = 3;
        Bullet_scr bullet_Scr = InitialBullet.GetComponent<Bullet_scr>();
        bullet_Scr.damage = GetBulletDamageFromList();
        bullet_Scr.numberText.text = bullet_Scr.damage.ToString();
        bullet_Scr.inClip = true;
        
        Clip.Add(InitialBullet);
        
        for (int i = 1; i < 11; i++)
        {
            Vector3 _newPoint = clipPositions[i].transform.position; // new(Clip[i].transform.position.x - bulletDistance, transform.position.y + 0.5f ,transform.position.z);
            GameObject newBullet = Instantiate(bulletPrefab,_newPoint,transform.rotation,transform);
            newBullet.layer = 3;
            bullet_Scr = newBullet.GetComponent<Bullet_scr>();
            bullet_Scr.inClip = true;
            bullet_Scr.damage = GetBulletDamageFromList();
            bullet_Scr.numberText.text = bullet_Scr.damage.ToString();
            Clip.Add(newBullet);
        }
        
    }
    public float GetBulletDamageFromList(){
        if(bulletList.Count > 0){
            Debug.Log(1);
            float _damage =  bulletList[0];
            bulletList.RemoveAt(0);
            return _damage;
        }
        else if( CacheClip.Count > 0){
            float _damage = CacheClip[(int)UnityEngine.Random.Range(0f, 10f)];
            return _damage;
        }
        else{
            return Mathf.Floor(UnityEngine.Random.Range(5f,30f));
        }
    }
  public void MoveBullets()
{
    TimeElapsed += Time.deltaTime;
    if (TimeElapsed >= 1 / rate)
    {
        TimeElapsed = 0;

        if (TimeElapsed <= 0.5f / rate)
        {
            Transform parentTransform = transform; // The common parent
            for (int i = 0; i < Clip.Count; i++)
            {
                if (i == 0)
                {
                    // Get the local min x position relative to the parent
                    float localMinX = parentTransform.InverseTransformPoint(GetComponent<Collider>().bounds.min).x;
                    Vector3 newLocalPosition = new(localMinX, Clip[i].transform.localPosition.y, Clip[i].transform.localPosition.z);
                    StartCoroutine(LerpObject(Clip[i], newLocalPosition, bulletDistance / (rate * 5)));
                }
                else
                {
                    // Calculate the local distance between bullets
                    float newDistance = Clip[i - 1].transform.localPosition.x - Clip[i].transform.localPosition.x;
                    // Vector3 newLocalPosition = new(Clip[i].transform.localPosition.x + (newDistance / rate), Clip[i-1].transform.localPosition.y, Clip[i].transform.localPosition.z);
                    Vector3 newLocalPosition = Clip[i-1].transform.localPosition;
                    StartCoroutine(LerpObject(Clip[i], newLocalPosition, bulletDistance / (rate * 5)));
                }
            }
        }
    }
}

IEnumerator LerpObject(GameObject obj, Vector3 targetLocalPosition, float duration)
{
    float elapsedTime = 0f;
    Vector3 startLocalPosition = obj.transform.localPosition; // Use local position

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        if (obj == null) yield break;
        obj.transform.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, t);
        yield return null;
    }

    obj.transform.localPosition = targetLocalPosition; // Ensure final position is accurate
}
}
