using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerPoint_scr : MonoBehaviour
{
    public GameObject bouncerPrefab;
    public GameObject bouncer = null;    
    public string sing = "+";
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool PlaceBouncer(GameObject _bouncer)
{
    if(bouncer != null) return false;
    bouncer = Instantiate(_bouncer, transform.position, transform.rotation);
    Debug.Log("instantiated");
    
    bouncer.transform.localScale = new Vector3( 2.5f, 2.5f, bouncer.transform.localScale.z);
    
    bouncer.GetComponent<Bouncer_scr>().dragable = false;
    
    Debug.Log(_bouncer.GetComponent<Bouncer_scr>().bounceNumber);
    
    bouncer.GetComponent<Bouncer_scr>().bounceNumber = _bouncer.GetComponent<Bouncer_scr>().bounceNumber;
    
    Debug.Log(bouncer.GetComponent<Bouncer_scr>().bounceNumber);
    bouncer.GetComponent<Bouncer_scr>().bouncerText.text ="AAAAAAAAAAAAAAAAA"; //bouncer.GetComponent<Bouncer_scr>().bounceNumber.ToString();
    return true;
}
}
