using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer_scr : MonoBehaviour
{
    public GameObject gun;
    public Material conveyerMaterial;
    public float scrollSpeed = 1f; // Speed at which the texture moves
    public float conveyerForceMultiplier = 5f; // Speed to apply to objects on the X-axis
    public float offset;
    private float conveyerForce;

    void Start()
    {
        conveyerMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {   
        conveyerForce = conveyerForceMultiplier * gun.GetComponent<GunScr>().rate;
        
        MoveConveyerBelt();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has tag "bullet"
        if (!collision.gameObject.CompareTag("bullet")){return;}

        float collisionX = collision.contacts[0].point.x;

        collision.gameObject.layer = 3; // sets the panelBullets layer to clipBullet,clipBullet doesnt collide with default

        Rigidbody otherRigidbody = collision.rigidbody;
        if (otherRigidbody != null){otherRigidbody.constraints = RigidbodyConstraints.None;}

        Vector3 centerOfThisObject = transform.position;
        collision.transform.position = centerOfThisObject;

        Vector3 newPosition = collision.transform.position;
        newPosition.x = collisionX;
        newPosition += transform.up * offset;
        collision.transform.position = newPosition;


        // Lock the y-position and all rotation constraints of the other object
        if (otherRigidbody != null)
        {   
            collision.transform.rotation = Quaternion.Euler(-40, 180, 0);
            otherRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            
        }

        Vector3 newVelocity = -collision.gameObject.transform.right * conveyerForce; // Adjust speed as needed
        otherRigidbody.velocity = newVelocity;
    }
       void MoveConveyerBelt()
    {
        float timeOffset = Time.time * scrollSpeed;
        conveyerMaterial.SetFloat("_TimeOffset", timeOffset);
    }

}
