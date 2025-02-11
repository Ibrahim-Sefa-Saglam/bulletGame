using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void GoToMenu() // ✅ This method should be public and parameterless
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(0);
    }
   public void RetryGame() // ✅ This method should be public and parameterless
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
