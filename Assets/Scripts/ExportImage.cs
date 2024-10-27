using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportImage : MonoBehaviour
{
    public static ExportImage Instance;
    public string fileName = "wallpaper by @ziaonder";
    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public static Texture2D textureToSave;

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
                Destroy(Instance.gameObject);
        }
        else
            Instance = this;
    }

    public void SaveTextureAsPNG()
    {
        byte[] bytes = textureToSave.EncodeToPNG();
        string filePath = Path.Combine(downloadsPath, fileName + " " + 
            UIController.Instance.scaleSlider.value.ToString("F3") + " scale" + ".png");

        File.WriteAllBytes(filePath, bytes);
    }
}
