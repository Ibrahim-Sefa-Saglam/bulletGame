using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GunScr : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletPoint;
    public GameObject clip;
    public GameObject globalDataObjet;
    public GlobalData_scr globalData_;
 
    public float horizontalSpeed = 5f;
    public float forwardSpeed = 5f;
    public float bulletLifetime = 3f;
    // public float rate = 1.5f; // works in reverse, is equal to the amout of time gun waits b4 shooting 
    public float maxRate = 20f; // viable for use 
    public float minRate = 0.2f; // just here to show
    public float rate = 1.5f; 

    void Start(){        
    
    globalData_ = globalDataObjet.GetComponent<GlobalData_scr>();

    forwardSpeed = 0;
    GetComponent<Rigidbody>().velocity = transform.forward * forwardSpeed;
    }
    void Update()
    {
        Move();                
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("dummy")){
            forwardSpeed=0f;
            GameOver();
        }
    }
    public void Move()
    {
        forwardSpeed = 5;
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow))
        {   
            moveDirection = transform.right; // Move to the object's local right
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -transform.right; // Move to the object's local left
        }                
        moveDirection *= horizontalSpeed;
        moveDirection += transform.forward * forwardSpeed;
        transform.position += moveDirection  * Time.deltaTime;
    }
     void GameOver(){
         Vector3 direction = (-transform.forward + transform.up).normalized; 

        // Apply the force to the rigidbody in the calculated direction
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * 80, ForceMode.Impulse);
        }
        rate = 0f;
    }
    public void FireFromClip(float damage){// fires a bullet with damage as given parameter
         if (bullet != null && bulletPoint != null)
        {
            Vector3 offset = bulletPoint.transform.forward * 0.6f;
            GameObject bulletInstance = Instantiate(bullet, bulletPoint.transform.position + offset, bulletPoint.transform.rotation);
            bullet_scr bulletInstanceScr = bulletInstance.GetComponent<bullet_scr>();

            Debug.Log("gun_scrFireFromClip parameter damage: "+ damage);
            bulletInstanceScr.Initialize(damage,bulletLifetime);  
            bulletInstanceScr.Travel(bulletPoint.transform.forward,forwardSpeed+bulletInstanceScr.velocity);
        }
        else
        {
            Debug.LogWarning("Bullet prefab or bulletPoint is not assigned: FireFromCLip.");
        }    
    }
  }
