using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunScr : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletPoint;
    public GameObject clip;
    public float horizontalMaxSpeed = 5f;
    public float forwardSpeed = 5f;
    public float bulletLifetime = 3f;
    public float maxRate = 20f;  
    public float minRate = 0.2f;
    public float rate = 1.5f; 
    
    public float horizontalSpeed;
    private Vector3 startTouchPosition; // Position when swipe starts
    private Vector3 currentTouchPosition; // Current touch position
    public Vector3 startingPosition;
    public Vector3 targetPosition;
    private bool isSwiping = false;   // To track the initi


    private void Start() {
        startingPosition = transform.position;
    }

    public void Update()
    { 
        TrackMouseMovement();
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<IPlayerInteractable>(out var interactable)){                    
            GateScript gateScript = other.gameObject.GetComponent<GateScript>();
            interactable.InteractPlayer(gameObject);
        }
    }
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
            Bullet_scr bulletInstanceScr = bulletInstance.GetComponent<Bullet_scr>();

            bulletInstanceScr.Initialize(damage,bulletLifetime);  
            bulletInstanceScr.Travel(bulletPoint.transform.forward,forwardSpeed+bulletInstanceScr.velocity);
        }
        else
        {
        }    
    }
    private void HandleSwipeInput()
{
    // For mobile touch input
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            startTouchPosition = touch.position; // Store the start position of the swipe
            currentTouchPosition = startTouchPosition; // Initialize the current position
            isSwiping = false;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
                if (touch.position == new Vector2(currentTouchPosition.x, currentTouchPosition.y)) // Check if position has changed
                {
                        Debug.Log("6");

                    isSwiping = false; // Stop swiping when the mouse button is released
                                       // Smoothly reduce velocity to zero when mouse is released
                    float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
                    GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
                }
                else
                {
                    isSwiping = true; // Swiping detected

                    float diff = touch.position.x - currentTouchPosition.x;
                    float deltaInput = diff / Screen.width;

                    horizontalSpeed = deltaInput * horizontalMaxSpeed;
                    
                    currentTouchPosition = touch.position; // Update the current position

                    // Move the object based on the swipe direction
                    if (diff < 0 && transform.position.x > startingPosition.x - 5)
                    {
                        GetComponent<Rigidbody>().AddForce(-transform.right * horizontalMaxSpeed, ForceMode.Impulse);
                    }
                    else if (diff > 0)
                    {
                        if (transform.position.x < startingPosition.x + 5)
                        {
                            GetComponent<Rigidbody>().AddForce(transform.right * horizontalMaxSpeed, ForceMode.Impulse);
                        }
                    }
                }
                                    
                    if(!isSwiping){
                        ;
                        float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
                        GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);       
                    }


            }
        else if (touch.phase == TouchPhase.Ended)
        {

            isSwiping = false; // Stop movement when touch ends
            // Smoothly reduce velocity to zero when touch ends
            float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
            GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }
    }
    // For mouse input (for testing purposes on desktop)
    else if (Input.GetMouseButtonDown(0))
    {
        startTouchPosition = Input.mousePosition; // Store the initial touch position
        currentTouchPosition = startTouchPosition; // Initialize the current position
        isSwiping = false;
    }
    else if (Input.GetMouseButton(0))
    {
        if (Input.mousePosition.x != currentTouchPosition.x) // Check if the position has changed
        {
            isSwiping = true; // Swiping detected

            float diff = Input.mousePosition.x - currentTouchPosition.x;
            float deltaInput = diff / Screen.width;

            horizontalSpeed = deltaInput * horizontalMaxSpeed;

            currentTouchPosition = Input.mousePosition; // Update the current position

            // Move the object based on the swipe direction
            if (diff < 0)
            {
                if (transform.position.x > startingPosition.x - 5)
                {
                    GetComponent<Rigidbody>().AddForce(-transform.right * horizontalMaxSpeed, ForceMode.Impulse);
                }
                else
                {
                    transform.position = new Vector3(startingPosition.x - 5, transform.position.y, transform.position.z);
                }
            }
            else if (diff > 0)
            {
                if (transform.position.x < startingPosition.x + 5)
                {
                    GetComponent<Rigidbody>().AddForce(transform.right * horizontalMaxSpeed, ForceMode.Impulse);
                }
                else
                {
                    transform.position = new Vector3(startingPosition.x + 5, transform.position.y, transform.position.z);
                }
            }
        }
        else
        {
            isSwiping = false; // Swiping detected
            // Smoothly reduce velocity to zero when mouse is stationary
            float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
            GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        }
    }
    else if (Input.GetMouseButtonUp(0))
    {
        isSwiping = false; // Stop swiping when the mouse button is released
        // Smoothly reduce velocity to zero when mouse is released
        float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
    }
    else
    {
        // Smoothly reduce velocity to zero when no input is given
        float lerp = Mathf.Lerp(GetComponent<Rigidbody>().velocity.x, 0, 15f * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = new Vector3(lerp, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
    }
}
    void Movea()
    {
         if (Input.GetMouseButton(0)) // Mouse click for desktop
        {
            Debug.Log(1);
            Debug.Log(transform.position.x);
            Vector3 mousePosition = Input.mousePosition;
            // Convert the mouse position to world position
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Debug.Log(3 + "  "+mousePosition);
            targetPosition = new Vector3(mousePosition.x, transform.position.y, transform.position.z); // Only change X position

            if(mousePosition.x > startingPosition.x - 5 || mousePosition.x < startingPosition.x + 5){
                Debug.Log(2);
                Debug.Log(targetPosition.x);
            transform.position = Vector3.Lerp(transform.position, targetPosition, horizontalSpeed * Time.deltaTime);
            }
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

        inputPosition = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, transform.position.z));
        Vector3 newPosition = new(inputPosition.x, transform.position.y, transform.position.z);

        // Move object to follow the input position (only on the X axis)
        if (newPosition.x > startingPosition.x + 5)
        {
            newPosition = new Vector3(startingPosition.x + 5, transform.position.y, transform.position.z);
        }
        else if (newPosition.x < startingPosition.x - 5)
        {
            newPosition = new Vector3(startingPosition.x - 5, transform.position.y, transform.position.z);
        }
        
        transform.position = newPosition;
    }
}
}
