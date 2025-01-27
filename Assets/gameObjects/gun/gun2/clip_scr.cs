using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clip_scr : MonoBehaviour
{
    public GameObject gun; 
    public GameObject bullet;
    public GameObject bulletGenerationPoint; 
    public List<float> bulletList = new List<float>();
    public List<float> cachedList = new List<float>();
    public Transform[] initialPositions;
    private GameObject[] initialBullets = new GameObject[3];
    public float rate;
    public float clipLength;
    private float lastFireTime = 0f;


    void Start()
    {
        rate = gun.GetComponent<GunScr>().rate;
    }        
    
    void OnEnable() {
        initialBullets[0].GetComponent<Rigidbody>().velocity = 4 * rate * transform.right;
        initialBullets[1].GetComponent<Rigidbody>().velocity = 4 * rate * transform.right;
        initialBullets[2].GetComponent<Rigidbody>().velocity = 4 * rate * transform.right;        
    }
    // Update is called once per frame
    void Update()
    {
        rate = gun.GetComponent<GunScr>().rate;
        if (Time.time >= lastFireTime + (1f / rate)) // 1 / rate gives the time delay between shots
        {
            float nextBulletDamage;
            if(bulletList.Count >0){                
                nextBulletDamage = bulletList[0];
                bulletList.RemoveAt(0);
                Debug.Log("clipscr.Update nextBulletDamage: "+ nextBulletDamage);

            }
            else{ 
                nextBulletDamage = cachedList[(int)Mathf.Floor(Random.Range(1,11f))];
            }
            GenerateClipBullet(bulletGenerationPoint.transform.position,nextBulletDamage);
            lastFireTime = Time.time; // Update last fire time to current time
        }
    }
    
    private void OnTriggerEnter(Collider other) { // every time a clipBullet hits, execute FireFromClip
        if(other.gameObject.CompareTag("bullet")){
            Destroy(other.gameObject);
            float bulletDamage = other.gameObject.GetComponent<Bullet_scr>().damage;
            GunScr gunScr = gun.GetComponent<GunScr>();
            Debug.Log("clipscr.OnTriggerEnter/FireFromClip bulletDamage: "+ bulletDamage);
            gunScr.FireFromClip(bulletDamage);
        }        
    }
    
    GameObject GenerateClipBullet(Vector3 genPoint, float bulletDamage){ // generates bullet in clips
        
        GameObject bulletInstance = Instantiate(bullet, genPoint, transform.rotation);
        bulletInstance.transform.SetParent(transform);
        Bullet_scr bulletInstanceScr = bulletInstance.GetComponent<Bullet_scr>();
        

        bulletInstanceScr.Initialize(bulletDamage,5); // generates a bullet at the generation point with arbitrary life
        bulletInstanceScr.Travel(transform.right, 4*rate);
        
        bulletInstance.layer = 3;
        bulletInstance.GetComponent<Rigidbody>().useGravity = false;
        bulletInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        return bulletInstance;            
    }
    public void InitializeClipBullets(){
        for (int i = 0; i < 3; i++)
        {
            if(bulletList.Count>=i){
                initialBullets[i] = GenerateClipBullet(initialPositions[i].transform.position,bulletList[i]);
            }
            else{
                initialBullets[i] = GenerateClipBullet(initialPositions[i].transform.position, Random.Range(1,30.99f));
            }
                initialBullets[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }    
}
