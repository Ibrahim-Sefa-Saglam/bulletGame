using System.Security.Cryptography;
using Systems.RunnerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Dummy_scr : MonoBehaviour, IBulletInteractable,IPlayerInteractable
{
    public GameObject starPrefab;
    public GameObject InteractableObject { get ; set ; }
    public Renderer[] componentRenderers;
    private Color _rgbColorValue = new Color(1, 1, 0); // Default to RGB(255, 255, 0) // 80 -> 200 red, 125 -> 0 green & blue 
    public TextMeshProUGUI  numberText;
    public float dummyVal;
    public float dummyMaxVal = 400;
    public float dummyMinVal = 0;
    
    private bool exponantial =  false;

    
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
        
    }   

    void Initialize(){
        if(componentRenderers == null ||componentRenderers.Length != 3 )return;

        dummyVal =  Mathf.FloorToInt( Random.Range(50,dummyMaxVal));
        numberText.text = dummyVal.ToString();

        foreach (Renderer item in componentRenderers)
        {
            float redValue = dummyVal/dummyMaxVal * 120 + 80f;  
            float otherValues = (dummyMaxVal-dummyVal)/ dummyMaxVal * 125 + 80f;
            item.material.color =  new Color(redValue/225, otherValues/225,  otherValues/225); 
        }       
    }
    public void InitializeDummy(float exponentialValue, float maxParameter){
        exponantial = true;
        dummyMaxVal = maxParameter;
        if(componentRenderers == null ||componentRenderers.Length != 3 ){
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
        Rigidbody rb = collidedObject.GetComponent<Rigidbody>();
        GunScr gunScript = collidedObject.GetComponent<GunScr>(); 
        rb.isKinematic = false;
        rb.constraints  =  RigidbodyConstraints.None;
        rb.AddForce(direction * 80, ForceMode.Impulse);
        gunScript.rate = 0f;
        gunScript.forwardSpeed=0f;
        GameStateHandler.SetState(GameStateHandler.GameStates.Lose);
    }  
    void HandleBulletCollision(IBullet bullet)
    {
        float bulletDamage = bullet.BulletInfo.Damage;
        dummyVal = dummyVal - bulletDamage;
        
        numberText.text  = dummyVal.ToString();
    
        float redValue = dummyVal/dummyMaxVal * 120 + 80f;  
        float otherValues = (dummyMaxVal-dummyVal)/ dummyMaxVal * 125f + 80f;
        
        _rgbColorValue = new Color(redValue/225, otherValues/225,  otherValues/225); // RGB: (dynamic red, 255 green, 0 blue)

        foreach (Renderer item in componentRenderers)
        {            
            if (item.material.color != null)
            {
                item.material.color = _rgbColorValue;
            }
        }
        if(dummyVal<=0){
            GameObject newStar = Instantiate(starPrefab, transform.position, new(-90,0,90,0));
            newStar.transform.SetParent(transform.parent, true);
            Destroy(gameObject);
        }
    }
    public void InteractBullet(System.Action callback, IBullet bullet, out bool isDestroy)
    {
        isDestroy = false;
        if(bullet != null){
            HandleBulletCollision(bullet);  
            isDestroy = true;
            callback();
        }
    }
    public void InteractPlayer(GameObject player){
        if(player.CompareTag("Player")) {
            HandlePlayerCollision(player);
        }
    }

}
