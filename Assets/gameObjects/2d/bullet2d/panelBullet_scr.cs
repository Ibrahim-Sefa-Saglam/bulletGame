using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelBullet_scr : MonoBehaviour
{
    private Rigidbody rb;
    public TextMeshProUGUI  numberText;
    public float customGravity = -10f; // Custom gravity force 
    public float damage = -1;
    private bool isCounting = false; // Whether the countdown has started
    private float countdown = 20f;
    public float panelTime = 0;


    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        Initialize();
    }

    // Update is called once per frame
    
    void Update()
    {
        panelTime += Time.deltaTime;
        rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration);

        // Check if the collider is disabled and the countdown hasn't started
        if (!GetComponent<Collider>().enabled && !isCounting)
        {
            isCounting = true; // Start counting
        }

        // If the countdown has started, decrement the timer
        if (isCounting)
        {
            countdown -= Time.deltaTime;

            // Destroy the object after 20 seconds
            if (countdown <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
   void OnCollisionEnter(Collision collision)
    {
        
        Transform blueSphere = collision.transform.Find("blueSphere");
        if (blueSphere != null)
        {
            Transform canvas = blueSphere.Find("Canvas");
            if (canvas != null)
            {
                Transform textObject = canvas.Find("Text");
                if (textObject != null)
                {
                    // Access the TextMeshPro component
                    TextMeshProUGUI tmpText = textObject.GetComponent<TextMeshProUGUI>();
                    if (tmpText != null)
                    {
                        string textValue = tmpText.text;
                        if (float.TryParse(textValue, out float floatValue))
                        {
                            damage += floatValue;
                            numberText.text = damage.ToString();   
                        }
                    }
                    else
                    {
                        Debug.LogWarning("TextMeshProUGUI component not found on 'Text'.");
                    }
                }
                else
                {
                    Debug.LogWarning("'Text' object not found under 'Canvas'.");
                }
            }
            else
            {
                Debug.LogWarning("'Canvas' object not found under 'blueSphere'.");
            }
        }
        
    }
    void Initialize(){
        damage = Mathf.FloorToInt( Random.Range(5,30.99f));
        if(damage<5 || damage>30){
            damage  = 10f;
        }
        numberText.text = damage.ToString();        
    }
    public void InitializeInClip(float bulletValue)
    {
        damage = bulletValue;
    }
}
 