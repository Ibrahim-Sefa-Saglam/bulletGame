using Systems.RunnerSystem;
using UnityEngine;
using UnityEngine.EventSystems;  

public class BouncerBehaviors : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBulletInteractable
{
    private GameObject _collidedObject;
    public GameObject InteractableObject { get; set; }
    private Bouncer _bouncerAttributes;
    private Camera _mainCamera;
    public bool isDragging = false;
    public bool dragable = true;

    public bool isDropped
    {
        get { return _bouncerAttributes.BouncerData.isDropped;}
        set{ _bouncerAttributes.BouncerData.isDropped = value; }
    }
    void Start()
    {
        InteractableObject = gameObject;
        _mainCamera = Camera.main;
        _bouncerAttributes = GetComponent<Bouncer>();
    }    
    void OnTriggerEnter(Collider collision)
    {
       
        // Check the tag of the collided object
        if(collision.gameObject.CompareTag("bouncer")){
            _collidedObject = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("bouncerPoint_+") || collision.gameObject.CompareTag("bouncerPoint_x")) // if collided with bouncerPoint_+/bouncerPoint_x assing new object, when the assingment is one with onDrop change the bouncerPoints tag, 
        {            
                _collidedObject = collision.gameObject;            
        }
        else
        {            
            _collidedObject = null;
            return;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!dragable) return;

        
        isDragging = true;
        isDropped = false;
        OnDrag(eventData);
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        
        if (isDragging)
        {
            // Move the object to the mouse or touch position
            Vector3 screenPoint = new Vector3(eventData.position.x, eventData.position.y, _mainCamera.WorldToScreenPoint(_bouncerAttributes.originalPosition).z);
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(screenPoint);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, _bouncerAttributes.originalPosition.z);
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

        if (_collidedObject != null)
        {
            if (_collidedObject.CompareTag("bouncer"))
            {      
                Bouncer collidedObjectScript = _collidedObject.GetComponent<Bouncer>();       
                if(collidedObjectScript.sing != _bouncerAttributes.sing || !Mathf.Approximately(collidedObjectScript.bounceNumber, _bouncerAttributes.bounceNumber)) {
                    transform.position = _bouncerAttributes.originalPosition;
                    return;
                }
                
                collidedObjectScript.IncrementBouncerNumber();
                _bouncerAttributes.DestroySelf();
            }
            else if (_collidedObject.CompareTag("bouncerPoint_+"))
            {
                _collidedObject.GetComponent<Collider>().enabled = false;

                transform.position = _collidedObject.transform.position;
                GetComponent<BouncerData>().bouncerPosition = _collidedObject.transform.position;
                
                dragable = false;
                
                gameObject.layer = 0;
            }
            else if(_collidedObject.CompareTag("bouncerPoint_x")){} // WILL BE FILLED LATER
        }
        else
        {
            transform.position = _bouncerAttributes.originalPosition;
        }
        isDropped = false;
    }
    
    public void HandlePanleBulletCollision(IBullet bullet){
        bullet.BulletInfo.Damage += _bouncerAttributes.bounceNumber;
        bullet.BulletInfo.BulletText = bullet.BulletInfo.Damage.ToString();
        StartCoroutine(_bouncerAttributes.ChangeColorCoroutine());
    }
    public void InteractBullet(System.Action callback, IBullet bullet, out bool isDestroy)
    {
        Debug.Log(21243);
        isDestroy = false;        
        if(!isDropped) return;
        if(bullet == null) return;
        HandlePanleBulletCollision(bullet);
    }


}
