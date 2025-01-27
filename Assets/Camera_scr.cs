using UnityEngine;

public class Camera_scr : MonoBehaviour
{
    public GameObject gun; // Reference to the gun object
    public GameObject clip; // Reference to the gun object
    public GameObject panelBulletGenerator; // Reference to the gun object
    public float targetRelativeX = 0f; // Relative position on the X-axis
    public float targetRelativeY = 0f; // Relative position on the Y-axis
    public float targetRelativeZ = 0f; // Relative position on the Z-axis
    public float targetRotationX = 90f; // Target X-axis rotation
    public float targetRotationY = 90f; // Target Y-axis rotation
    public float transitionSpeed = 2f; // Speed of the camera transition
    private Vector3 targetPositionOffset; // Target position relative to the gun
    private Quaternion targetRotation; // Target rotation of the camera
    public string cameraState = "canvas"; // Whether the camera is currently transitioning

    
    void Start()
    {
        gun.GetComponent<GunScr>().enabled = false;        
        clip.GetComponent<Clip_scr>().enabled = false;
        
        // Initialize the target position offset and rotation
        targetPositionOffset = new Vector3(targetRelativeX, targetRelativeY, targetRelativeZ);
        targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
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
            Vector3 desiredPosition = gun.transform.position + gun.transform.TransformDirection(targetPositionOffset);
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
                clip.GetComponent<Clip_scr>().enabled = true;
                panelBulletGenerator.GetComponent<panelBulletGenerator_scr>().enabled = false;

            }
        }
        else if(cameraState == "following")
        {
            transform.SetPositionAndRotation(gun.transform.position + gun.transform.TransformDirection(targetPositionOffset), gun.transform.rotation * targetRotation);
        }
        
    }
    
}
