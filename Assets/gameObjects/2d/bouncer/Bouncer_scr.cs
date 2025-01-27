using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;  // For handling UI events

public class Bouncer_scr : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector3 origin;           // Stores the object's original position
    private GameObject collidedObject; // Stores the object it collides with
    private Camera mainCamera;        // Reference to the main camera
    public TextMeshProUGUI bouncerText;
    public bool isDragging = false;    // Indicates if the object is being dragged
    public bool isDropped = false;    // Indicates if the object has been dropped
    public bool dragable = true;    // Indicates if the object has been dropped
    public float bounceNumber;
    public string sing = "+";

    void Start()
    {
        mainCamera = Camera.main;
        origin = transform.position;
        bounceNumber = 1;
        bouncerText.text = "+" + bounceNumber.ToString();
    }

    // This function is triggered when the pointer is pressed down (mouse click or touch)
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!dragable) return;

        
        isDragging = true;
        isDropped = false;
        OnDrag(eventData);
        // Optionally: Store the offset from the pointer to the object
        // Vector3 offset = transform.position - eventData.position;
    }

    // This function is triggered while the pointer is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        
        if (isDragging)
        {
            // Move the object to the mouse or touch position
            Vector3 screenPoint = new Vector3(eventData.position.x, eventData.position.y, mainCamera.WorldToScreenPoint(origin).z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, origin.z);
        }
    }

    // This function is triggered when the pointer is released
    public void OnPointerUp(PointerEventData eventData)
    {
        if(!dragable) return;

        isDragging = false;
        isDropped = true; // this value will be set to false in OnDrop
        OnDrop();
    }

    void OnTriggerEnter(Collider collision)
    {


        // Check the tag of the collided object
        if(collision.gameObject.CompareTag("bouncer")){

            collidedObject = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("bouncerPoint"))
        {
            if(collision.gameObject.GetComponent<BouncerPoint_scr>().bouncer != null) {                
                collidedObject = collision.gameObject.GetComponent<BouncerPoint_scr>().bouncer;
            }
            else{
                collidedObject = collision.gameObject;
            }
        }
        else
        {
            
            collidedObject = null;
        }

    }

    private void OnDrop()
    {
        if (!isDropped) return;

        if (collidedObject != null)
        {
            if (collidedObject.CompareTag("bouncer"))
            {      
                Bouncer_scr collidedObjectScript = collidedObject.GetComponent<Bouncer_scr>();       
                if(collidedObjectScript.sing != sing || collidedObjectScript.bounceNumber != bounceNumber) {
                    transform.position = origin;
                    return;
                }
                
                collidedObjectScript.IncrementNumber(bounceNumber);
                Destroy(this.gameObject);
            }
            else if (collidedObject.CompareTag("bouncerPoint"))
            {
                BouncerPoint_scr bouncerPointScr = collidedObject.GetComponent<BouncerPoint_scr>();
                
                if(bouncerPointScr.sing != sing) {
                    transform.position = origin;
                    return;
                }
                bool isPlaced = bouncerPointScr.PlaceBouncer(this.gameObject);
                if(isPlaced)
                {
                    Destroy(this.gameObject);
                }
                else{
                    transform.position = origin;
                }
            }
        }
        else
        {
            transform.position = origin;
        }
        isDropped = false;
    }

    public void IncrementNumber(float _number)
    {
        bounceNumber ++;
        bouncerText.text = sing + bounceNumber.ToString();
    }
}
