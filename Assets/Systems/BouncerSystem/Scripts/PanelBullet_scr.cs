using TMPro;
using UnityEngine;

public class PanelBullet_scr : MonoBehaviour
{
    public TextMeshProUGUI  numberText;
    public float customGravity = -10f; // Custom gravity force 
    public float damage = -1;
    private bool isCounting = false; 
    public bool inPanel = true;
    private float countdown = 20f;

    void Start()
    {   
        if(inPanel){
            InitializeInPanel();
        }
    }
    void Update()
    {   
        if(inPanel){     
            GetComponent<Rigidbody>().AddForce(Vector3.up * customGravity, ForceMode.Acceleration);
            if (isCounting)
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    void InitializeInPanel(){
        damage = Mathf.FloorToInt( Random.Range(5,30.99f));
        if(damage<5 || damage>30){
            damage  = 10f;
        }
        numberText.text = damage.ToString();        
    }
    public void InitializeInClip(float bulletValue)
    {
        damage = bulletValue;
        numberText.text = damage.ToString();
    }    
  
  
}
 