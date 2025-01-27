using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelBulletGenerator_scr : MonoBehaviour
{
    public GameObject gun;             // Bullet prefab to instantiate
    public GameObject panelBullet;             // Bullet prefab to instantiate
    public GameObject dataTrackerObject;             // Bullet prefab to instantiate
    public GameDataTracker gameDataTracker;
    public RectTransform spawnpoint;           // Spawn point for bullets
    
    public float generationRateMultiplier = 1.5f;          // Time interval in seconds between bullet generation
    public float generationRate;          // Time interval in seconds between bullet generation

    private void Start()
    {
        gameDataTracker = dataTrackerObject.GetComponent<GameDataTracker>();

        if (generationRateMultiplier > 0)
        {
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
            generationRate = generationRateMultiplier * gun.GetComponent<GunScr>().rate;
            GeneratePanelBullet(); // Call the bullet generation method
            yield return new WaitForSeconds(1/generationRate); // Wait for the specified rate before generating the next bullet
        }
    }

    void GeneratePanelBullet()
    {
        if (panelBullet != null && spawnpoint != null)
        {
            gameDataTracker.AddGenerationRate(1);
            float randomAddition =  Random.Range(-1f,1);
            Vector3 randomizedSpawnPoint = new Vector3(spawnpoint.position.x+randomAddition,spawnpoint.position.y,spawnpoint.position.z);
            GameObject bulletInstance = Instantiate(panelBullet, randomizedSpawnPoint, spawnpoint.rotation);
            bulletInstance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            Debug.LogWarning("panelBullet or spawnpoint is not assigned.");
        }
    }
}
