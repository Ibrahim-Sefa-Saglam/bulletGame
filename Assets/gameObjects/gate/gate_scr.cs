using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    // Attributes
    public float gateType; // if 1 range; if 0 rate
    public float gateVal;
    public float gateMaxVal = 100;
    public float gateMinVal = -100;
    private Color rgbColorValue = new Color(1, 1, 0, 240/255f); // Default to RGB(255, 255, 0)
    public Material gateColor;    
    public TextMeshProUGUI  numberText;
    public TextMeshProUGUI  gateTypeText;
    public Renderer gateRenderer;
    public Renderer[] outlineRenderers;
   
   [SerializeField]
    protected GameObject player;

    public float hoverAmplitude = 0.5f; // The maximum height difference of the hover
    public float hoverFrequency = 5f;
    private Vector3 startPosition; // To store the initial position of the object

    
    [SerializeField]
    private GunScr playerScr;
    [SerializeField]
    private Bullet_scr bulletScr;

    void Start()
    {        
        startPosition = transform.position;
        InitializeGate();
    }

    void Update()
    {
        Hover();
        setOutlineColor();

    }

   void OnTriggerEnter(Collider other){


    if (other.CompareTag("Player"))
    {
        playerScr = other.GetComponent<GunScr>();
        // Handle player collision logic
        PlayerCollision();
    }
    else if (other.CompareTag("bullet"))
    {
        // Handle bullet collision logic
        bulletScr = other.GetComponent<Bullet_scr>();
        BulletCollision(bulletScr.damage); 
        Destroy(other.gameObject);

    }
}
    void PlayerCollision()
    {
        // Placeholder for player collision logic
        if(gateType == 0){
            if(playerScr.rate + gateVal/50 >= playerScr.maxRate ) {playerScr.rate =playerScr.maxRate;}
            else if(playerScr.rate + gateVal/50 <= playerScr.minRate ){ playerScr.rate =playerScr.minRate;}
            else {playerScr.rate += gateVal/50; }
        }
        else  if(gateType == 1){
            if( (playerScr.bulletLifetime + gateVal/50 )<= 0 ) {playerScr.bulletLifetime = 0.1f;}
            else{ playerScr.bulletLifetime += gateVal/50;}
        }
        foreach (Renderer _renderer in outlineRenderers)
        {
            _renderer.material.color = new (1,1,1);
        }
        Destroy(gameObject);
    }

    void BulletCollision(float value)
    {
        gateVal += value;

        // Clamp the value between gateMinVal and gateMaxVal
        gateVal = Mathf.Clamp(gateVal, gateMinVal, gateMaxVal);
        
        // update text
        numberText.text  = gateVal.ToString();
        
        // Update the color based on gateVal
        if (gateVal > 0)
        {
            float redValue = Mathf.Clamp01(1 - (gateVal / (gateMaxVal-25))); // Scale red based on positive value // EĞER SAYI > 0 İSE KIRMIZI DEĞERİNİ ORANLA AZALT
            rgbColorValue = new Color(redValue, 1, 0, 240/255f); // RGB: (dynamic red, 255 green, 0 blue)
        }
        else if (gateVal < 0)
        {
            float greenValue = Mathf.Clamp01(1 - Mathf.Abs(gateVal / (gateMinVal+75))); // Scale green based on negative value
            rgbColorValue = new Color(1, 0, 0, 240/255f); // RGB: (255 red, dynamic green, 0 blue)// EĞER SAYI < 0 İSE YEŞİL DEĞERİNİ ORANLA AZALT
        }
        else
        {
            rgbColorValue = new Color(1, 1, 0, 240/255f); // Reset to yellow if gateVal is 0
        }

        // Apply the updated color to the material
        if (gateRenderer.material.color != null)
        {
            gateRenderer.material.color = rgbColorValue;
        }
        
    }


    void InitializeGate(){

        gateType = 0;// to have it initialised
        gateType =  Mathf.FloorToInt( Random.Range(0,1.999f));
        
        if(gateType == 0){
            gateTypeText.text = "rate";
        }
        else if(gateType == 1){
            gateTypeText.text = "range";
        }
        else{
            Debug.Log("gateType is false: "+gateType.ToString());
        }

        gateVal = Mathf.FloorToInt( Random.Range(-50.9f, 50.9f));
        numberText.text = gateVal.ToString();

        // Initialize gate color if a material is assigned
        if (gateColor != null)
        {
               if (gateVal > 0)
        {
            float redValue = Mathf.Clamp01(1 - (gateVal / (gateMaxVal-25))); // Scale red based on positive value // EĞER SAYI > 0 İSE KIRMIZI DEĞERİNİ ORANLA AZALT
            rgbColorValue = new Color(redValue, 1, 0, 240/255f); // RGB: (dynamic red, 255 green, 0 blue)
        }
        else if (gateVal < 0)
        {
            rgbColorValue = new Color(1, 0, 0, 240/255f); // RGB: (255 red, dynamic green, 0 blue)// EĞER SAYI < 0 İSE YEŞİL DEĞERİNİ ORANLA AZALT
        }
        else
        {
            rgbColorValue = new Color(1, 1, 0, 240/255f); // Reset to yellow if gateVal is 0
        }
        }
        
         if (gateRenderer != null && gateRenderer.material != null)
        {
            // Create an instance of the material assigned to the Gate
            Material gateMaterialInstance = new Material(gateRenderer.material);

            // Change the color of the instance material (e.g., red)
            gateMaterialInstance.color = rgbColorValue;

            // Assign the new material instance back to the Gate's Renderer
            gateRenderer.material = gateMaterialInstance;
        }
   
    }

     void Hover()
    {
        // Calculate the new position
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;

        // Apply the new position while maintaining original X and Z
        transform.parent.position = startPosition + new Vector3(0f, hoverOffset, 0f);
    }
    void setOutlineColor(){
        if( outlineRenderers == null ){
            Debug.Log("outline renderers is flawed");
            return;
        }
        foreach (Renderer item in outlineRenderers)
        {
             Material gateMaterialInstance = new Material(gateRenderer.material);
             item.material = gateMaterialInstance;
             
             if(gateVal>0){
             item.material.color = new Color(0,0.8f,0);
             }
             else if(gateVal<0){
             item.material.color = new Color(0.8f,0,0);

             }

        }
    }
}
