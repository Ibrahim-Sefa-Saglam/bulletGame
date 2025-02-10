using System.Collections;
using UnityEngine;

public class PanelBulletGenerator_scr : MonoBehaviour
{
    public GameObject panelBullet;
    public GameObject gun;
    public GunScr GunScr;
    public RectTransform spawnpoint;           // Spawn point for bullets
    public float generationRate;          // Time interval in seconds between bullet generation

    private void Start()
    {
      
            GunScr = gun.GetComponent<GunScr>();
            StartCoroutine(GeneratePanelBulletAtRate());
     
    }
    private IEnumerator GeneratePanelBulletAtRate()
    {
        // Infinite loop to generate bullets at the specified rate
        while (true)
        {
            generationRate =  GunScr.rate ;
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
