using System.Collections;
using TMPro;
using UnityEngine;

    public class BouncerData
    {
        public static int BouncerCount = 0;
        private int _bouncerNumber;
        private Vector3 _bouncerTransform;
    
        public BouncerData (ref int bouncerNumber, ref Vector3 bouncerTransform)
        {
            _bouncerNumber = bouncerNumber;
            BouncerCount++;
            _bouncerTransform = bouncerTransform;
        }
    
    }
    public class BouncerPrefabAttributes: MonoBehaviour
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
            InitializeBouncer(transform.position,1);
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
            // Transition to white
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

        public void InitializeBouncer(Vector3 startingPositionParameter, int bouncerNumberParameter)
        {
            originalPosition = startingPositionParameter;
            bouncerText.text = "+" + bouncerNumberParameter.ToString();
            BouncerData = new BouncerData(ref bounceNumber, ref originalPosition);
            Behaviors = GetComponent<BouncerBehaviors>();
        }
    }
