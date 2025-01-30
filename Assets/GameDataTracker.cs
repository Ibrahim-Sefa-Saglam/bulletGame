using System.Collections.Generic;
using UnityEngine;

public class GameDataTracker_scr : MonoBehaviour
{
    public List<float> last10BulletDamages = new List<float>(); // Store the damage of the last 10 bullets
    public List<float> allBulletDamages = new List<float>();   // Store all delivered bullet damages
    public List<float> last10PanelTimes = new List<float>();   // Store the panel times of the last 10 bullets
    public List<float> allPanelTimes = new List<float>();      // Store all panel times
    public float totalGenerationRate = 0f;  // Total accumulated generation rate
    public float totalDeliveringRate = 0f;  // Total accumulated delivering rate
    public int generationCount = 0;        // Count of generation rate updates
    public int deliveringCount = 0;        // Count of delivering rate updates
    public float totalTime = 0;


    private void Update() {
        totalTime += Time.deltaTime;        
    }
    // Method to add bullet damage
    public void AddBulletDamage(float damage)
    {
        allBulletDamages.Add(damage);

        if (last10BulletDamages.Count >= 10)
        {
            last10BulletDamages.RemoveAt(0); // Remove the oldest entry
        }
        last10BulletDamages.Add(damage);
    }

    // Method to add panel time
    public void AddPanelTime(float panelTime)
    {
        allPanelTimes.Add(panelTime);

        if (last10PanelTimes.Count >= 10)
        {
            last10PanelTimes.RemoveAt(0); // Remove the oldest entry
        }
        last10PanelTimes.Add(panelTime);
    }

    // Method to add generation rate
    public void AddGenerationRate(float rate)
    {
        totalGenerationRate += rate;
        generationCount++;
    }

    // Method to add delivering rate
    public void AddDeliveringRate(float rate)
    {
        totalDeliveringRate += rate;
        deliveringCount++;
    }

    // Method to calculate the average damage of the last 10 bullets
    public float GetAverageLast10BulletDamage()
    {
        if (last10BulletDamages.Count == 0) return 0f;
        float sum = 0f;
        foreach (float damage in last10BulletDamages)
        {
            sum += damage;
        }
        return sum / last10BulletDamages.Count;
    }

    // Method to calculate the average damage of all bullets
    public float GetAverageAllBulletDamage()
    {
        if (allBulletDamages.Count == 0) return 0f;
        float sum = 0f;
        foreach (float damage in allBulletDamages)
        {
            sum += damage;
        }
        return sum / allBulletDamages.Count;
    }

    // Method to calculate the average panel time of the last 10 bullets
    public float GetAverageLast10PanelTime()
    {
        if (last10PanelTimes.Count == 0) return 0f;
        float sum = 0f;
        foreach (float time in last10PanelTimes)
        {
            sum += time;
        }
        return sum / last10PanelTimes.Count;
    }

    // Method to calculate the average panel time of all bullets
    public float GetAverageAllPanelTime()
    {
        if (allPanelTimes.Count == 0) return 0f;
        float sum = 0f;
        foreach (float time in allPanelTimes)
        {
            sum += time;
        }
        return sum / allPanelTimes.Count;
    }

    // Method to calculate the average generation rate
    public float GetAverageGenerationRate()
    {
        if (generationCount == 0) return 0f;
        return generationCount / totalTime;
    }

    // Method to calculate the average delivering rate
    public float GetAverageDeliveringRate()
    {
        if (deliveringCount == 0) return 0f;
        return deliveringCount / totalTime;
    }
    
   public void DisplayData()
{
    Debug.Log("===== Game Data Tracker =====");
    
    Debug.Log("Delivered count: "+ deliveringCount);
    
    // Bullet Damage Averages
    Debug.Log("Average Damage of Last 10 Bullets: " + GetAverageLast10BulletDamage());
    Debug.Log("Average Damage of All Bullets: " + GetAverageAllBulletDamage());
    
    // Panel Time Averages
    Debug.Log("Average Panel Time of Last 10 Bullets: " + GetAverageLast10PanelTime());
    Debug.Log("Average Panel Time of All Bullets: " + GetAverageAllPanelTime());
    
    // Generation and Delivering Rates
    Debug.Log("Average Generation Rate: " + GetAverageGenerationRate());
    Debug.Log("Average Delivering Rate: " + GetAverageDeliveringRate());
    
    Debug.Log("=============================");
}

}
