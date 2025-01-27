
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class bullet_scr : MonoBehaviour
{
    public float damage;
    public float velocity = 15f;
    [SerializeField]
    private float lifetime = 3f;    
    public TextMeshProUGUI  numberText;
    public Collider bulletCollider;
    public Vector3 direction = Vector3.forward;
    private float elapsedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (bulletCollider == null)
        {
            Debug.LogWarning("Collider not assigned in the Inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet when it collides with something
        if (collision.collider == bulletCollider)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float _lifetime)
    {
        lifetime = _lifetime;
    }

    public void Initialize(float _damage, float life)
    {
        damage = _damage;
        Debug.Log("bullet_scr.Initialize local damage: "+damage);
        Debug.Log("bullet_scr.Initialize parameter damage: "+_damage);
        lifetime = life;
        numberText.text = damage.ToString();
    }

    public void Travel(float _velocity){
     velocity = _velocity;   
     GetComponent<Rigidbody>().velocity = direction * velocity;
    }
    public void Travel(Vector3 _direction,float _velocity){
        velocity = _velocity;
        direction = _direction;
        GetComponent<Rigidbody>().velocity = direction * velocity;
    }
}
