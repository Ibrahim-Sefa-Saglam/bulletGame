
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet_scr : MonoBehaviour
{
    public GameObject bulletEffect;
    public TextMeshProUGUI  numberText;
    public Vector3 direction = Vector3.forward;
    
    [SerializeField]
    private float lifetime = 3f;    
    public float damage;
    public float velocity = 15f;
    private float elapsedTime = 0f;
    public float panelGravity = -10f; // Custom gravity force 
    public bool inPanel;
    public bool inClip = false;
    public bool onConveyer = false;
    public Vector3 targetPositionOnClip;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         if(inPanel){     
            GetComponent<Rigidbody>().AddForce(Vector3.up * panelGravity, ForceMode.Acceleration);          
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime && !inPanel && !inClip)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<IBulletInteractable>(out var interactable))
        {   
            interactable.InteractBullet(CreateCollisionEffect,gameObject, out var isDestroy);
            if(isDestroy) Destroy(gameObject);
        }
    }
    public void Initialize(float _lifetime)
    {
        inPanel = false;
        lifetime = _lifetime;
        GetComponent<Rigidbody>().useGravity = false;        
    }
    public void Initialize(float _damage, float life)
    {
        inPanel = false;
        damage = _damage;
        lifetime = life;
        numberText.text = damage.ToString();
        GetComponent<Rigidbody>().useGravity = false;
    }
    public void InitializeInPanel(){
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        inPanel = true;
        damage = Mathf.FloorToInt( Random.Range(5,30.99f));
        if(damage<5 || damage>30){
            damage  = 10f;
        }
        numberText.text = damage.ToString();        
    }
    public void InitializeInClip(float bulletValue)
    {   
        inPanel = true;
        damage = bulletValue;
        numberText.text = damage.ToString();
    }  
    public void Travel(float _velocity){
     velocity = _velocity;   
     GetComponent<Rigidbody>().velocity = direction * velocity;
    }
    public void Travel(Vector3 _direction,float _velocity){
        velocity = _velocity;
        direction = _direction;
        GetComponent<Rigidbody>().velocity = direction * velocity;
    }
    public void CreateCollisionEffect(){
        Vector3 effectPoint = new(transform.position.x,transform.position.y, transform.position.z);
        Quaternion effectRotation = new(transform.rotation.x,transform.rotation.y, Random.Range(0,180),transform.rotation.w);
        GameObject effect = Instantiate(bulletEffect,effectPoint,effectRotation);
        Destroy(effect, 0.05f);       
    }
}
