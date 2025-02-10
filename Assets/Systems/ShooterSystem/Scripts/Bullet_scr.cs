using System;
using Systems.RunnerSystem;
using TMPro;
using UnityEngine;
public class BulletScr : MonoBehaviour, IBullet
{
    public ParticleSystem particleEffect;
    public TextMeshProUGUI  numberText;
    public Vector3 direction = Vector3.forward;
    public BulletInfo BulletInfo { get; set; } = new BulletInfo();
    public Vector3 targetPositionOnClip;
    public Rigidbody rb;

    
    private ref float LifetimeRef => ref BulletInfo.Lifetime;
    private ref float DamageRef => ref BulletInfo.Damage;
    private ref float VelocityRef => ref BulletInfo.Velocity;  
    private float _elapsedTime = 0f; 
    public float panelGravity = -10f; // Custom gravity force 
    public bool inPanel = false;
    public bool inClip = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    void Update()
    {
         if(inPanel){     
            rb.AddForce(Vector3.up * panelGravity, ForceMode.Acceleration);          
         }
         _elapsedTime += Time.deltaTime;

         numberText.text = BulletInfo.BulletText;
          if (_elapsedTime >= LifetimeRef && !inPanel && !inClip)
          {
              Destroy(this.gameObject);
          }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<IBulletInteractable>(out var interactable))
        {   
            interactable.InteractBullet(CreateCollisionEffect,this, out var isDestroy);
            if(isDestroy) Destroy(gameObject);
        }
    }
    public void InitializeForClip(float damage)
    {
        gameObject.layer = 3; // clipBullet
        inPanel = false;
        inClip = true;
        DamageRef = damage;
        BulletInfo.BulletText = DamageRef.ToString();
        rb.useGravity = false;        
    }
    public void InitializeForFire(float damage, float life)
    {
        BulletInfo.Damage = damage;
        BulletInfo.Lifetime = life;
        
        inPanel = false;
        DamageRef = damage;
        LifetimeRef = life;
        BulletInfo.BulletText = DamageRef.ToString(); 
        rb.useGravity = false;
    }
    public void InitializeInPanel(){
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;

        float newDamage = 1;
        BulletInfo newBulletInfo = new BulletInfo(newDamage);
        BulletInfo = newBulletInfo;
        inPanel = true;
    
        BulletInfo.BulletText = DamageRef.ToString();        
        numberText.text = DamageRef.ToString();
    }
    private void CreateCollisionEffect(){
       ParticleSystem particleInstance = Instantiate(particleEffect,transform.position,transform.rotation);
       particleInstance.Play();
       Destroy(particleEffect,.3f);
    }

    public void Fire(BulletInfo bulletInfo)
    {
        
        rb.useGravity = false;

        inPanel = false;
        
        DamageRef = bulletInfo.Damage;
        LifetimeRef = bulletInfo.Lifetime;
        VelocityRef = bulletInfo.Velocity;
        BulletInfo.BulletText = DamageRef.ToString();
        rb.velocity = gameObject.transform.forward * VelocityRef;

    }
    public void Fire() {}
    public void Hit(Action hitCallback){}
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
