using UnityEngine;

public class GunScr : MonoBehaviour
{
    public GameObject camera;
    public GameObject bullet;
    public GameObject bulletPoint;
    public GameObject clip;
    public Rigidbody rb;
    public ClipScript clipScript;
    private Vector3 startingPosition;
    public float forwardSpeed = 5f;
    public float bulletLifetime = 3f;
    public float maxRate = 20f;  
    public float minRate = 0.2f;
    public float rate = 0f; 
    public float horizontalSpeed;
    private float elapsedTime = 0f;
    private bool isSwiping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        clipScript = clip.GetComponent<ClipScript>();
        this.enabled = false;
        clipScript.enabled = false;
        GameManager.SetGun(gameObject);
        startingPosition = transform.position;
    }

    public void Update()
    { 
        elapsedTime+= Time.deltaTime;
        if(elapsedTime >= 0.6f)TrackMouseMovement();
        
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<IPlayerInteractable>(out var interactable)){
            interactable.InteractPlayer(gameObject);
        }
    }
    // for testing    
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
    public void FireFromClip(float damage){// fires a bullet with damage as given parameter
         if (bullet != null && bulletPoint != null)
         {
             Vector3 offset = bulletPoint.transform.forward * 0.6f;
             GameObject bulletInstance = Instantiate(bullet, bulletPoint.transform.position + offset, bulletPoint.transform.rotation);

             BulletScr bulletInstanceScr = bulletInstance.GetComponent<BulletScr>();
            

             bulletInstanceScr.InitializeForFire(damage,bulletLifetime);
             bulletInstanceScr.Fire(bulletInstanceScr.BulletInfo);
         }
    }
     void TrackMouseMovement()
    {

        

        Vector3 inputPosition = Vector3.zero;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isSwiping = false;
        }

        if (isSwiping)
        {
            if (Input.touchCount > 0)
            {
                inputPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButton(0))
            {
                inputPosition = Input.mousePosition;
            }

            float depth = Mathf.Abs(camera.GetComponent<Camera>().WorldToScreenPoint(transform.position).z);
            inputPosition = camera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, depth));
            Vector3 newPosition = new(transform.position.x,transform.position.y,inputPosition.z);// new(transform.position.x, transform.position.y, transform.position.z );

            if (newPosition.z > startingPosition.z + 5)
            {
                newPosition = new Vector3(transform.position.x , transform.position.y, startingPosition.z + 5);
            }
            else if (newPosition.z < startingPosition.z - 5)
            {
                newPosition = new Vector3(transform.position.x, transform.position.y, startingPosition.z - 5);
            }
            transform.position = newPosition + transform.forward * forwardSpeed;;
        
        }
    
}
     
    public void StartAllGunBehavior()
        {
            clipScript = clip.GetComponent<ClipScript>();
            this.enabled = true;
            clipScript.enabled = true;
        }
            
}
    