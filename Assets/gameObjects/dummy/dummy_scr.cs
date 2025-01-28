using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dummy_scr : MonoBehaviour
{
    public float dummyVal;
    public float dummyMaxVal = 400;
    public float dummyMinVal = 0;

    private Color rgbColorValue = new Color(1, 1, 0); // Default to RGB(255, 255, 0) // 80 -> 200 red, 125 -> 0 green & blue 
    public Material dummyMaterial;    
    public TextMeshProUGUI  numberText;
    public Renderer[] componentRenderers;
    private bool exponantail =  false;

    // Start is called before the first frame update
    void Start()
    {
   
    if(!exponantail){
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
    void OnTriggerEnter(Collider other){

    if (other.CompareTag("bullet"))
    {
        // Handle bullet collision logic        
        BulletCollision(other.gameObject.GetComponent<Bullet_scr>().damage); // Example value; replace with actual value as needed
        Destroy(other.gameObject);

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
        exponantail = true;
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

     void BulletCollision(float value)
    {
        dummyVal -= value;

        // Clamp the value between dummyMinVal and dummyMaxVal
        dummyVal = Mathf.Clamp(dummyVal, dummyMinVal, dummyMaxVal);
        
        // update text
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
}
