using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy_scr : MonoBehaviour, IBulletInteractable,IPlayerInteractable
{
    public GameObject InteractableObject { get ; set ; }
    public Renderer[] componentRenderers;
    public Material dummyMaterial;    
    private Color rgbColorValue = new Color(1, 1, 0); // Default to RGB(255, 255, 0) // 80 -> 200 red, 125 -> 0 green & blue 
    public TextMeshProUGUI  numberText;
    public float dummyVal;
    public float dummyMaxVal = 400;
    public float dummyMinVal = 0;

    private bool exponantial =  false;

    public bool CanInteract { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
    void Start()
    {
    InteractableObject = gameObject;
    if(!exponantial){
     Initialize();
     }      
   
    }

    // Update is called once per frame
    void Update()
    {
        if(dummyVal<=0){
            Destroy(gameObject);

        }
    }   

    void Initialize(){
        if(componentRenderers == null ||componentRenderers.Length != 3 ){
            return;
        }
        
        dummyVal =  Mathf.FloorToInt( Random.Range(50,dummyMaxVal));
        numberText.text = dummyVal.ToString();

        foreach (Renderer item in componentRenderers)
        {
            float redValue = dummyVal/dummyMaxVal * 120 + 80f;  
            float otherValues = (dummyMaxVal-dummyVal)/ dummyMaxVal * 125 + 80f;
            item.material.color =  new Color(redValue/225, otherValues/225,  otherValues/225); 

        }       

    }
    public void Initialize(float exponentialValue, float maxParameter){
        exponantial = true;
        dummyMaxVal = maxParameter;
        if(componentRenderers == null ||componentRenderers.Length != 3 ){
            Debug.Log("componentRenderer has issues");
            return;
        }
        
        dummyVal =  Mathf.FloorToInt( exponentialValue  + Random.Range(-5,10));
        numberText.text = dummyVal.ToString();

        foreach (Renderer item in componentRenderers)
        {
            float redValue = dummyVal/dummyMaxVal * 120 + 80f;  
            float otherValues = (dummyMaxVal-dummyVal)/ dummyMaxVal * 125 + 80f;
            item.material.color =  new Color(redValue/225, otherValues/225,  otherValues/225); 

        }       

    }
    void HandlePlayerCollision(GameObject collidedObject){
    
        Vector3 direction = (-collidedObject.transform.forward + collidedObject.transform.up).normalized; 
        if (collidedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(direction * 80, ForceMode.Impulse);
        }
        collidedObject.GetComponent<GunScr>().rate = 0f;
        collidedObject.GetComponent<GunScr>().forwardSpeed=0f;
    }  
    void HandleBulletCollision(GameObject bullet)
    {
        float bulletDamage = bullet.GetComponent<Bullet_scr>().damage;
     
        dummyVal -= bulletDamage;
        dummyVal = Mathf.Clamp(dummyVal, dummyMinVal, dummyMaxVal);
        
        numberText.text  = dummyVal.ToString();
    
        float redValue = dummyVal/dummyMaxVal * 120 + 80f;  
        float otherValues = (dummyMaxVal-dummyVal)/ dummyMaxVal * 125f + 80f;
        
        rgbColorValue = new Color(redValue/225, otherValues/225,  otherValues/225); // RGB: (dynamic red, 255 green, 0 blue)

        foreach (Renderer item in componentRenderers)
        {            
            if (item.material.color != null)
            {
                item.material.color = rgbColorValue;
            }
        }
    }
   public void InteractBullet(System.Action callback, GameObject collidedObject, out bool isDestroy)
    {
        isDestroy = false;
        if(collidedObject.CompareTag("bullet")){
            HandleBulletCollision(collidedObject);  
            isDestroy = true;
            callback();
        }
    }
    public void InteractPlayer(GameObject player){
        if(player.CompareTag("player")) {
            HandlePlayerCollision(player);
        }
    }

}
