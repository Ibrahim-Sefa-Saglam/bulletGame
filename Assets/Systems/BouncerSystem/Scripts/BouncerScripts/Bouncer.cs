using System;
using System.Collections;
using Systems.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
    public class BouncerData
    {
        public static int BouncerCount = 0;
        [SerializeField]
        public int bouncerNumber;
        [SerializeField]
        public Vector3 bouncerPosition;
        [SerializeField]
        public Quaternion bouncerRotation;
    
        public BouncerData (ref int bouncerNumber, Vector3 bouncerPosition , Quaternion bouncerRotation)
        {
            this.bouncerNumber = bouncerNumber;
            BouncerCount++;
            this.bouncerPosition = bouncerPosition;
            this.bouncerRotation = bouncerRotation;
        }
    
    }
    public class Bouncer: MonoBehaviour
    {
        public TextMeshProUGUI bouncerText;
        public Renderer blueSphereRenderer;
        public BouncerData BouncerData; 
        public Color originalColor;
        public Color[] colors;
        public Vector3 originalPosition;
        public string sing = "+";
        public int bounceNumber = 1;
        public BouncerBehaviors Behaviors;

        void Start()
        {
            originalPosition = transform.position;
            bouncerText.text = "+" + 1;
            Behaviors = GetComponent<BouncerBehaviors>();
            BouncerData = new BouncerData(ref bounceNumber, originalPosition , transform.rotation);
            GameSaveData.Instance.BouncerDataList.Add(BouncerData);
        }
        public void IncrementBouncerNumber()
        {
             bounceNumber ++;
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

            // Wait for 0.5 seconds

            // Transition back to the original color
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
            GameSaveData.Instance.BouncerDataList.Remove(BouncerData);
            Destroy(gameObject);
        }
        public static GameObject InstantiateBouncer(GameObject prefab, BouncerData bouncerData = null, Transform parent = null)
        {
            var newBouncer = Instantiate(prefab);
            if(parent != null)
            {
                newBouncer.transform.position = parent.position;
                newBouncer.transform.rotation = parent.rotation;
            }
            
            var bouncerScript =  newBouncer.GetComponent<Bouncer>();
            if (bouncerData != null)
            {
                 bouncerScript.BouncerData = bouncerData;
                bouncerScript.originalPosition = bouncerData.bouncerPosition;
                newBouncer.transform.position = bouncerData.bouncerPosition;
                newBouncer.transform.rotation = bouncerData.bouncerRotation;
                bouncerScript.bouncerText.text = "+" + bouncerData.bouncerNumber;
                bouncerScript.Behaviors = bouncerScript.gameObject.GetComponent<BouncerBehaviors>();
            }
            else
            {
                bouncerScript.BouncerData = new BouncerData(ref bouncerScript.bounceNumber, newBouncer.transform.position, newBouncer.transform.rotation);
            }
            
            return newBouncer;
        }
    }
