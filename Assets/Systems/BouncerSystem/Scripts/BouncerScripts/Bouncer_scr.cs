using System;
using System.Collections;
using Systems.RunnerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;  // For handling UI events

public class Bouncer_scr : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBulletInteractable
{
    public Vector3 origin;           // Stores the object's original position
    private GameObject collidedObject; // Stores the object it collides with
    public GameObject InteractableObject {get; set;}
    private Camera mainCamera;
            // Reference to the main camera
    public TextMeshProUGUI bouncerText;
    public Renderer blueSphereRenderer;
    public Color originalColor;
    public Color[] colors;
    public bool isDragging = false;    // Indicates if the object is being dragged
    public bool isDropped = false;    // Indicates if the object has been dropped
    public bool dragable = true;    // Indicates if the object has been dropped
    public float bounceNumber = 1;
    public string sing = "+"; // Later "x" will be added for multiplication

    void Start()
    {
        InteractableObject = gameObject;
        mainCamera = Camera.main;
        origin = transform.position;
        bouncerText.text = "+" + bounceNumber.ToString();
        originalColor = blueSphereRenderer.material.color;
    }    

    void OnTriggerEnter(Collider collision)
    {
       
        // Check the tag of the collided object
        if(collision.gameObject.CompareTag("bouncer")){

            collidedObject = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("bouncerPoint_+") || collision.gameObject.CompareTag("bouncerPoint_x")) // if collided with bouncerPoint_+/bouncerPoint_x assing new object, when the assingment is one with onDrop change the bouncerPoints tag, 
        {            
                collidedObject = collision.gameObject;            
        }
        else
        {            
            collidedObject = null;
            return;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!dragable) return;

        
        isDragging = true;
        isDropped = false;
        OnDrag(eventData);
        // Optionally: Store the offset from the pointer to the object
        // Vector3 offset = transform.position - eventData.position;
    }
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
    public void OnPointerUp(PointerEventData eventData)
    {
        if(!dragable) return;

        isDragging = false;
        isDropped = true; // this value will be set to false in OnDrop
        OnDrop();
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
            else if (collidedObject.CompareTag("bouncerPoint_+"))
            {
                bool isPlaced = PlaceBouncer(collidedObject);// bouncerPointScr.PlaceBouncer(this.gameObject);
                if(isPlaced)
                {
                    collidedObject.GetComponent<Collider>().enabled = false;
                    Destroy(gameObject);
                }
                else{
                    transform.position = origin;
                }
            }
            else if(collidedObject.CompareTag("bouncerPoint_x")){} // WILL BE FILLED LATER
        }
        else
        {
            transform.position = origin;
        }
        isDropped = false;
    }
    public bool PlaceBouncer(GameObject _bouncerPoint)
    {
        GameObject newBouncer = Instantiate(gameObject, _bouncerPoint.transform.position, _bouncerPoint.transform.rotation);
        
        newBouncer.transform.localScale = new Vector3( 2.5f, 2.5f, transform.localScale.z);
        
        newBouncer.GetComponent<Bouncer_scr>().dragable = false;
        newBouncer.layer = 0;
        newBouncer.GetComponent<Bouncer_scr>().bounceNumber = GetComponent<Bouncer_scr>().bounceNumber;
    
        return true;
    }
    public void IncrementNumber(float _number)
    {
        bounceNumber ++;
        bouncerText.text = sing + bounceNumber.ToString();
        if(bounceNumber>5) return;
        blueSphereRenderer.material.color = colors[(int)bounceNumber-1];
        originalColor = colors[(int)bounceNumber-1];
    }
    private IEnumerator ChangeColorCoroutine()
    {
        // Transition to white
        float timeElapsed = 0f;        
        float duration = 0.1f;
        Color startColor = blueSphereRenderer.material.color;
        while (timeElapsed < duration)
        {
            blueSphereRenderer.material.color = Color.Lerp(startColor, Color.white, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        blueSphereRenderer.material.color = Color.white;  // Ensure it ends exactly at white

        // Wait for 0.5 seconds

        // Transition back to the original color
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            blueSphereRenderer.material.color = Color.Lerp(Color.white, originalColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        blueSphereRenderer.material.color = originalColor;  // Ensure it ends exactly at the original color
    }
    public void HandlePanleBulletCollision(IBullet bullet){
        bullet.BulletInfo.Damage += bounceNumber;
        bullet.BulletInfo.BulletText = bullet.BulletInfo.Damage.ToString();
        StartCoroutine(ChangeColorCoroutine());
    }
    public void InteractBullet(System.Action callback, IBullet bullet, out bool isDestroy)
    {   
        isDestroy = false;        
        if(!isDropped) return;
        if(bullet == null) return; 
        HandlePanleBulletCollision(bullet);        
    }

    
}
