using System;
using System.Collections;
using System.Collections.Generic;
using Systems.RunnerSystem;
using UnityEngine;

public class Conveyer_scr : MonoBehaviour, IBulletInteractable
{
    public GameObject gun;
    public GameObject InteractableObject {get; set;}
    public Material conveyerMaterial;
    public GunScr gunScr;
    public float scrollSpeed = 1f; // Speed at which the texture moves
    public float conveyerForceMultiplier = 5f; // Speed to apply to objects on the X-axis
    public float offset;
    private float conveyerForce;

    public bool CanInteract { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    

    void Start()
    {
        InteractableObject = gameObject;
        conveyerMaterial = GetComponent<Renderer>().material;
        gunScr = gun.GetComponent<GunScr>();
    }

    void Update()
    {   
        conveyerForce = conveyerForceMultiplier * gunScr.rate;
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
    void HandlePanleBulletCollision(IBullet bullet){
        var bulletScr = bullet as BulletScr;
        if (bulletScr != null)
        {
            var bulletObj = bulletScr.gameObject;
            
            Rigidbody bulletRigidbody = bulletObj.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = Vector3.zero;
            Vector3 centerOfConveyer = transform.position;
            Vector3 newPosition = new(bulletObj.transform.position.x,centerOfConveyer.y,centerOfConveyer.z);
            bulletRigidbody.velocity = new(conveyerForce,0,0);
            bulletObj.layer = 9; // set to deafult to not interact with conveyer any more 
            
            newPosition += transform.up * offset;
            bulletObj.transform.SetPositionAndRotation(newPosition, new Quaternion(0,180,0,transform.rotation.w));
            bulletRigidbody.constraints = RigidbodyConstraints.None;
            bulletRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            bulletScr.onConveyer = true;
        }
   }

    public void InteractBullet(Action callback, IBullet bullet, out bool isDestroy)
    {
            isDestroy = false;
            if (bullet == null)return;
            HandlePanleBulletCollision(bullet);
    }
}
