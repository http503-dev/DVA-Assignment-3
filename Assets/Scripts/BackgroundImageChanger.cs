using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageChanger : MonoBehaviour
{
    public Image targetImage;            // Reference to the UI Image
    public Sprite[] sprites;             // List of sprites to cycle through
    public float interval = 3f;          // Time between changes (in seconds)

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (sprites.Length > 0 && targetImage != null)
        {
            targetImage.sprite = sprites[0];
        }
    }

    void Update()
    {
        if (sprites.Length == 0 || targetImage == null) return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % sprites.Length;
            targetImage.sprite = sprites[currentIndex];
        }
    }
}
