using UnityEngine;
using UnityEngine.UI;

public class BackgroundLooper : MonoBehaviour
{
    public Image backgroundImage; // Reference to the initial background image
    private Image nextBackgroundImage; // The duplicate image
    public float speed = 200f; // Movement speed

    private float screenHeight;

    void Start()
    {
        screenHeight = Screen.height;

        // Create only one additional copy on top
        nextBackgroundImage = Instantiate(backgroundImage, backgroundImage.transform.parent);
        nextBackgroundImage.transform.SetAsLastSibling();

        PositionNextBackground();
    }

    void Update()
    {
        // Move both backgrounds downward
        Vector3 movement = new Vector3(0, -speed * Time.deltaTime, 0);
        backgroundImage.rectTransform.anchoredPosition += (Vector2)movement;
        nextBackgroundImage.rectTransform.anchoredPosition += (Vector2)movement;

        // If the first image moves completely off-screen, swap references
        if (backgroundImage.rectTransform.anchoredPosition.y <= -screenHeight)
        {
            // Reposition the moved-out image on top
            (backgroundImage, nextBackgroundImage) = (nextBackgroundImage, backgroundImage);

            PositionNextBackground();
        }
    }

    void PositionNextBackground()
    {
        // Place the new image exactly above the current one
        nextBackgroundImage.rectTransform.anchoredPosition = new Vector2(
            backgroundImage.rectTransform.anchoredPosition.x,
            backgroundImage.rectTransform.anchoredPosition.y + screenHeight
        );
    }
}