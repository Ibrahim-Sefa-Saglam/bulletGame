using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer_scr : MonoBehaviour, IBulletInteractable
{
    public GameObject gun;
    public GameObject InteractableObject {get; set;}
    public Material conveyerMaterial;
    public float scrollSpeed = 1f; // Speed at which the texture moves
    public float conveyerForceMultiplier = 5f; // Speed to apply to objects on the X-axis
    public float offset;
    private float conveyerForce;

    public bool CanInteract { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    

    void Start()
    {
        InteractableObject = gameObject;
        conveyerMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {   
        conveyerForce = conveyerForceMultiplier * gun.GetComponent<GunScr>().rate;
        MoveConveyerBelt();
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
    void MoveConveyerBelt()
    {
        float timeOffset = - Time.time * scrollSpeed;
        conveyerMaterial.SetTextureOffset("_BaseMap", new(timeOffset,0));
    }
    void HandlePanleBulletCollision(){
 
   }

    public void InteractBullet(Action callback, GameObject bullet, out bool isDestroy)
    {
            isDestroy = false;
            if (!bullet.CompareTag("bullet"))return;
            if( bullet.GetComponent<Bullet_scr>().onConveyer) return;
            
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = Vector3.zero;
            Vector3 centerOfConveyer = transform.position;
            Vector3 newPosition = new(bullet.transform.position.x,centerOfConveyer.y,centerOfConveyer.z);
            bulletRigidbody.velocity = new(conveyerForce,0,0);
            bullet.layer = 9; // set to deafult to not interact with conveyer any more 
            
            newPosition += transform.up * offset;
            bullet.transform.SetPositionAndRotation(newPosition, new Quaternion(0,180,0,transform.rotation.w));
            bulletRigidbody.constraints = RigidbodyConstraints.None;
            bulletRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            bullet.GetComponent<Bullet_scr>().onConveyer = true;
            
            
            
    }
}
