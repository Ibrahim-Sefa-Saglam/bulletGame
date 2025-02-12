using System.Collections.Generic;
using Systems.RunnerSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class ClipScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject gun;
    public GunScr gunScr;
    public List<GameObject> clip = new();
    public List<float> bulletList = new();
    public List<float> cacheClip = new(); // fill from ClipFiller
    public float rate;
    public float bulletDistance;
    private float _timeElapsed;
    private float _timeElapsedForStarting;
 
    void Start()
    {
        gunScr = gun.GetComponent<GunScr>();
        rate = gunScr.rate;
        InitializeClipContent();
    }        
    void Update()
    {
        rate = gunScr.rate;
        _timeElapsedForStarting += Time.deltaTime;
        if(_timeElapsedForStarting > 0.9f) MoveBullets();
        CreateBullet();
        
    }   
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("bullet")){
            float bulletDamage = other.GetComponent<BulletScr>().BulletInfo.Damage;            
            GunScr gunScript = gun.GetComponent<GunScr>();
            gunScript.FireFromClip(bulletDamage);
            
            clip.Remove(other.gameObject);
            
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
        
        clip.Add(initialBullet);
        
        for (int i = 0; i <15; i++) // (int i = 1; i < 11; i++)
        {
            Vector3 point =  new(transform.position.x, transform.position.y + 1f ,clip[i].transform.position.z + bulletDistance); // clipPositions[i].transform.position;
            GameObject newBullet = Instantiate(bulletPrefab,point,transform.rotation,transform);
            newBullet.layer = 3;
            bulletScr = newBullet.GetComponent<BulletScr>();
            bulletScr.inClip = true;
            bulletScr.BulletInfo.Damage = GetBulletDamageFromList();
            bulletScr.BulletInfo.BulletText = bulletScr.BulletInfo.Damage.ToString();
            bulletScr.numberText.text = bulletScr.BulletInfo.Damage.ToString();
            clip.Add(newBullet);
        }
    }
    public float GetBulletDamageFromList(){

        if(bulletList.Count > 0){

            float damage =  bulletList[0];
            bulletList.RemoveAt(0);
            return damage;
        }
        else if( cacheClip.Count > 0){

            float damage = cacheClip[(int)UnityEngine.Random.Range(0f, cacheClip.Count)];
            return damage;
        }
        else{
            
            return Mathf.Floor(UnityEngine.Random.Range(3f,8f));
        }
    } 
    public void MoveBullets()
    {
        // Loop through each bullet in the list
        foreach (GameObject bullet in clip)
        {
            // Move the bullet to the right (positive X direction) by the specified speed
            bullet.transform.position += transform.right * (rate * Time.deltaTime) ;
        }
    }
    void CreateBullet(){
            _timeElapsed+= Time.deltaTime;
            if(clip.Count < 15  ){
                for(int i=0; i<15;i++){
                
                
                    Vector3 newPoint =  new(transform.position.x, transform.position.y + 1f ,clip[^1].transform.position.z  + bulletDistance); // clipPositions[i].transform.position;
                    GameObject newBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
                    BulletScr bulletScr = newBullet.GetComponent<BulletScr>();
                    bulletScr.InitializeForClip(GetBulletDamageFromList());
                    clip.Add(newBullet);
                }
            }
            else if (_timeElapsed >= 1/rate)
            {
                _timeElapsed = 0;
                Vector3 newPoint =  new(transform.position.x, transform.position.y + 1f ,clip[^1].transform.position.z  + bulletDistance); // clipPositions[i].transform.position;
                GameObject newBullet = Instantiate(bulletPrefab,newPoint,transform.rotation,transform);
                BulletScr bulletScr = newBullet.GetComponent<BulletScr>();
                newBullet.layer = 3;
                bulletScr = newBullet.GetComponent<BulletScr>();
                bulletScr.inClip = true;
                bulletScr.BulletInfo.Damage = GetBulletDamageFromList();
                bulletScr.BulletInfo.BulletText = bulletScr.BulletInfo.Damage.ToString();
                clip.Add(newBullet);
            }
        
        
}
}
