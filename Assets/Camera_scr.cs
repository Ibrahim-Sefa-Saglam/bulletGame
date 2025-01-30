using UnityEngine;

public class Camera_scr : MonoBehaviour
{
    public GameObject gun; // Reference to the gun object
    public GameObject clip; // Reference to the gun object
    public Vector3 PositionToGun; // Currently 0,16,-13
    public Quaternion targetRotation; // Currently 35,0,0
    public string cameraState = "canvas"; // Whether the camera is currently transitioning
    public float transitionSpeed = 2f; // Speed of the camera transition

    
    void Start()
    {
        gun.GetComponent<GunScr>().enabled = false;        
        clip.GetComponent<Clip_scr>().enabled = false;        
    }

    public void ActivateFollowMode()
    {
        // Start transitioning the camera to the gun
        cameraState = "isTransitioning";
        clip.GetComponent<Clip_scr>().InitializeClipBullets();
    }

    void LateUpdate()
    {
         if (cameraState == "isTransitioning")
        {
            // Smoothly move the camera to the desired position relative to the gun
            Vector3 desiredPosition = gun.transform.position + gun.transform.TransformDirection(PositionToGun);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * transitionSpeed);

            // Smoothly rotate the camera to the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, gun.transform.rotation * targetRotation, Time.deltaTime * transitionSpeed);

            // Check if the camera has approximately reached its target position and rotation
            if (Vector3.Distance(transform.position, desiredPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, gun.transform.rotation * targetRotation) < 0.5f)
            {
                // Lock the camera to follow the gun
                cameraState = "following";

                GunScr gunScript = gun.GetComponent<GunScr>();
                gunScript.enabled = true;
                if(clip.GetComponent<Clip_scr>() != null) clip.GetComponent<Clip_scr>().enabled = true;
                else{ Debug.Log("clipScr not found");}
                

            }
        }
        else if(cameraState == "following")
        {
            Vector3 newPosition = transform.position; // Keep current X position
            newPosition.y = gun.transform.position.y + gun.transform.TransformDirection(PositionToGun).y;
            newPosition.z = gun.transform.position.z + gun.transform.TransformDirection(PositionToGun).z;

            transform.SetPositionAndRotation(newPosition, gun.transform.rotation * targetRotation);
        }
        
    }
    
}
