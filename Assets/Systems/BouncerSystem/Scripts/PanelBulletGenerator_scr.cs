using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelBulletGenerator_scr : MonoBehaviour
{
    public GameObject panelBullet;
    public GameObject gun;
    public GunScr GunScr;
    public RectTransform spawnpoint;           // Spawn point for bullets
    public float generationRateMultiplier = 1f; // a variable for muliplying the gun.rate to match the desired bullet firing rate in gun
    public float generationRate;          // Time interval in seconds between bullet generation

    private void Start()
    {
        if (generationRateMultiplier > 0)
        {
            
            GunScr = gun.GetComponent<GunScr>();
            StartCoroutine(GeneratePanelBulletAtRate());
        }
        else
        {
            Debug.LogWarning("Generation rate must be greater than 0.");
        }
    }
    private IEnumerator GeneratePanelBulletAtRate()
    {
        // Infinite loop to generate bullets at the specified rate
        while (true)
        {
            generationRate = generationRateMultiplier* GunScr.rate ;
            BulletScr bulletScr = GeneratePanelBullet(); // Call the bullet generation method
            bulletScr.InitializeInPanel();
            yield return new WaitForSeconds(1/generationRate); // Wait for the specified rate before generating the next bullet
        }
    }

    BulletScr GeneratePanelBullet()
    {
        if (panelBullet && spawnpoint)
        {
            float randomAddition =  Random.Range(-1f,1);
            Vector3 randomizedSpawnPoint = new Vector3(spawnpoint.position.x+randomAddition,spawnpoint.position.y,spawnpoint.position.z);
            GameObject bulletInstance = Instantiate(panelBullet, randomizedSpawnPoint, spawnpoint.rotation,transform);
            return bulletInstance.GetComponent<BulletScr>();
        }
        else
        {
            Debug.LogWarning("panelBullet or spawnpoint is not assigned.");
            return null;
        }
    }
}
