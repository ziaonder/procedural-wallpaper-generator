using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public static TextureGenerator Instance;
    private int width = 500, height = 500;
    private Texture2D texture;
    public Texture2D Texture { get { return texture; } }
    private Sprite sprite;
    [SerializeField] private SpriteRenderer sRenderer;
    public FlexibleColorPicker fcp;
    private Color colorA, colorB;
    private float scale = 0.001f;

    private void Awake()
    {
        if(Instance != null)
        {
            if (Instance != this)
                Destroy(Instance.gameObject);
        }
        else
            Instance = this;
    }

    private void OnEnable()
    {
        UIController.OnValueChange += ChangeValue;
        UIController.OnGenerate += CreateSprite;
    }

    private void OnDisable()
    {
        UIController.OnValueChange -= ChangeValue;
        UIController.OnGenerate -= CreateSprite;
    }

    private void GenerateTexture()
    {
        texture = new Texture2D(width, height);
        float value;
        Color color;

        for (int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                value = Mathf.PerlinNoise(x * scale, y * scale);
                color = Color.Lerp(colorA, colorB, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    private void CreateSprite()
    {
        GenerateTexture();
        Texture2D previewTexture = CropTexture(texture, 500, 500);
        Rect rect = new Rect(0f, 0f, 500, 500);
        Vector2 pivot = new Vector2(0, 1);
        sprite = Sprite.Create(previewTexture, rect, pivot);
        sRenderer.sprite = sprite;
    }

    private Texture2D CropTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        // Create a new empty texture with the target dimensions
        Texture2D result = new Texture2D(targetWidth, targetHeight);

        // Copy the pixels from the source texture starting from (0, 0)
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // Copy pixel from the original texture at (x, y)
                Color color = source.GetPixel(x, y);
                result.SetPixel(x, y, color);
            }
        }

        result.Apply(); // Apply changes to the texture
        return result;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight);

        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                Color newColor = source.GetPixelBilinear((float)x / targetWidth, (float)y / targetHeight);
                result.SetPixel(x, y, newColor);
            }
        }

        result.Apply();
        return result;
    }

    private void ChangeValue(Color colorA, Color colorB, float scale, int res)
    {
        this.colorA = colorA;
        this.colorB = colorB;
        this.scale = scale;

        switch (res)
        {
            case 0:
                width = 1920;
                height = 1080;
                break;
            case 1:
                width = 1366;
                height = 768;
                break;
            case 2:
                width = 1080;
                height = 1920;
                break;
            case 3:
                width = 768;
                height = 1366;
                break;
            default:
                width = 1920;
                height = 1080;
                break;
        }
    }
}
