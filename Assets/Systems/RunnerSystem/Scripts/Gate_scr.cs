using System.Collections;
using System.Collections.Generic;
using Systems.RunnerSystem;
using TMPro;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class GateScript : MonoBehaviour, IBulletInteractable, IPlayerInteractable
{
    private Color rgbColorValue = new(1, 1, 0, 240/255f); // Default to RGB(255, 255, 0)
    public Material gateColor;
    public TextMeshProUGUI  numberText;
    public TextMeshProUGUI  gateTypeText;
    public Renderer gateRenderer;
    public Renderer[] outlineRenderers;
    public GameObject InteractableObject { get ; set ; }

    public float gateType; // if 1 range; if 0 rate
    public float gateVal;
    public float gateMaxVal = 100;
    public float gateMinVal = -100;
   
   [SerializeField]
    protected GameObject player;

    public float hoverAmplitude = 0.5f; // The maximum height difference of the hover
    public float hoverFrequency = 5f;
    private Vector3 startPosition; // To store the initial position of the object

    [SerializeField]
    private GunScr wplayerScr;
    [SerializeField]
    private BulletScr bulletScr;
    public bool CanInteract { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    void Start()
    {   
        InteractableObject = gameObject;
        startPosition = transform.position;
        InitializeGate();
    }
    void Update()
    {
        Hover();
        SetOutlineColor();

    } 
    void ChangeGateColor()
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
    void SetOutlineColor(){
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

    private IEnumerator ScaleBounce()
    {
        Transform parentTransform = transform.parent; // Reference the parent

        // Store original scale
        Vector3 originalScale = parentTransform.localScale;

        // Increase Y scale to 1.1
        parentTransform.localScale = new Vector3(originalScale.x, 1.1f, originalScale.z);

        // Wait for 0.15 seconds
        yield return new WaitForSeconds(0.15f);

        // Reset Y scale back to 1
        parentTransform.localScale = originalScale;
    }
    void HandlePlayerCollision(GameObject player)
    {       
        GunScr playerScript = player.GetComponent<GunScr>();
         if(gateType == 0){
            if(playerScript.rate + gateVal/50 >= playerScript.maxRate ) {playerScript.rate =playerScript.maxRate;}
            else if(playerScript.rate + gateVal/50 <=  playerScript.minRate ){ playerScript.rate = playerScript.minRate;}
            else {playerScript.rate += gateVal/50; }
        }
        else  if(gateType == 1){
            if( ( playerScript.bulletLifetime + gateVal/50 )<= 0 ) { playerScript.bulletLifetime = 0.1f;}
            else{  playerScript.bulletLifetime += gateVal/50;}
        }
         foreach (Renderer _renderer in outlineRenderers)
        {
            _renderer.material.color = new (1,1,1);
        }
        Destroy(gameObject);
        
    }
    void HandleBulletCollision(IBullet bullet)
    {
        float BulletDamage = bullet.BulletInfo.Damage;

        gateVal += BulletDamage;
        gateVal = Mathf.Clamp(gateVal, gateMinVal, gateMaxVal);    
        numberText.text  = gateVal.ToString();        
        ChangeGateColor();
    }
    public void InteractBullet(System.Action callback, IBullet bullet, out bool isDestroy)
    {   
        isDestroy = false;
        if (bullet != null)
        {
            HandleBulletCollision(bullet);
            StartCoroutine(ScaleBounce());
            isDestroy = true;
            callback(); 
        }
    }
    public void InteractPlayer(GameObject collidedObject)
    {
        if(collidedObject.CompareTag("Player")) {
            HandlePlayerCollision(collidedObject);
        }
    }
}
