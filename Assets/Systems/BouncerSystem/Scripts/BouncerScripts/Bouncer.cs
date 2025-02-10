using System;
using System.Collections;
using Systems.SaveSystem;
using TMPro;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
    public class BouncerData
{
    private static int _bouncerCount = 0;

    [SerializeField]
    public int bouncerNumber;
    public Transform bouncerTransform;// only carries the position, you can give the transform of another object without changing with any other property other than position and rotation
    [SerializeField]
    public Vector3 bouncerPosition;
    [SerializeField]
    public Quaternion bouncerRotation;
    [SerializeField]
    public bool isDragable;
    [SerializeField]
    public string sing;

    public BouncerData(ref int bouncerNumber, Transform bouncerTransform, bool isDragable, ref string sing)
    {
        _bouncerCount++; // Increment global count
        this.bouncerNumber = bouncerNumber;
        this.bouncerTransform = bouncerTransform;
        this.isDragable = isDragable;
        this.sing = sing;
    }

    public void SerializeData()
    {
     bouncerPosition = bouncerTransform.position;   
     bouncerRotation = bouncerTransform.rotation;   
    }

   
}

    public class Bouncer: MonoBehaviour
    {
        public TextMeshProUGUI bouncerText;
        public Renderer blueSphereRenderer;
        public BouncerData BouncerData { get; set; } 
        public Color originalColor;
        public Color[] colors;
        public Vector3 originalPosition;
        public string sing = "+";
        public int bounceNumber = 1;
        void Start()
        {
            BouncerData ??= new BouncerData(ref bounceNumber, transform, true, ref sing);
            originalPosition = transform.position;
            bouncerText.text = sing + bounceNumber;
        }
        public void IncrementBouncerNumber()
        {
             bounceNumber ++;
             BouncerData.bouncerNumber = bounceNumber;
             bouncerText.text =  sing +  bounceNumber.ToString();
             if( bounceNumber>5) return;
             blueSphereRenderer.material.color =  colors[(int) bounceNumber-1];
             originalColor =  colors[(int) bounceNumber-1];
        }
        public IEnumerator ChangeColorCoroutine()
        {
            float timeElapsed = 0f;        
            float duration = 0.1f;
            Color startColor =   blueSphereRenderer.material.color;
            while (timeElapsed < duration)
            {
                  blueSphereRenderer.material.color = Color.Lerp(startColor, Color.white, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            blueSphereRenderer.material.color = Color.white;  // Ensure it ends exactly at white

            timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                  blueSphereRenderer.material.color = Color.Lerp(Color.white,   originalColor, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            blueSphereRenderer.material.color =   originalColor;  // Ensure it ends exactly at the original color
        }

        public void DestroySelf()
        {
            GameSaveData.Instance.BouncerDataList.Remove( GameSaveData.Instance.BouncerDataList.Find(b => b == BouncerData));
            Destroy(this.gameObject);
        }
        public static GameObject InstantiateBouncer(GameObject prefab, BouncerData bouncerData = null, Transform newBouncerTransform = null)
        {
            var newBouncer = Instantiate(prefab);
            var bouncerScript =  newBouncer.GetComponent<Bouncer>();
            if(newBouncerTransform != null) // if the newBouncerTransform is given set the position and the rotation to the given transforms pos. and rot. 
            {
                newBouncer.transform.position = newBouncerTransform.position;
                newBouncer.transform.rotation = newBouncerTransform.rotation;
            }
            
            if (bouncerData != null)// if the bouncer data is given, set the new objects properties 
            {
                newBouncer.transform.position = bouncerData.bouncerPosition;
                bouncerData.bouncerTransform  = newBouncer.transform;
                newBouncer.transform.rotation = bouncerData.bouncerRotation;
                
                bouncerScript.BouncerData = bouncerData;
                bouncerScript.bounceNumber = bouncerData.bouncerNumber;
                bouncerScript.sing = bouncerData.sing;
                bouncerScript.bouncerText.text = bouncerData.sing + bouncerData.bouncerNumber;
                bouncerScript.blueSphereRenderer.material.color =  bouncerScript.colors[(int) bouncerScript.bounceNumber-1];
                bouncerScript.originalColor = bouncerScript.blueSphereRenderer.material.color;

            }
            else // if the bouncer data is null, construct the new data with the current bouncer's properties
            {
                bouncerScript.BouncerData = new BouncerData(ref bouncerScript.bounceNumber, newBouncer.transform, true,ref bouncerScript.sing);
            }
            
            return newBouncer;
        }
    }
