using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightSensor : MonoBehaviour
{
    public RenderTexture lightCheckTexture;
    public float lightLevel;

    public float deathLevel;
    public float maxDeathLevel;

    public Slider deathSlider;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        deathLevel = 0;
        deathSlider.maxValue = maxDeathLevel;
    }

    void Update()
    {
        LightCheck();
        
        DeathLevel();
    }

    public void LightCheck()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0,
            RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color[] colors = temp2DTexture.GetPixels(1);

        lightLevel = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            //lightLevel += (0.02126f * colors[i].r) + (0.07152f * colors[i].g) + (0.00722f * colors[i].b);
            lightLevel += (colors[i].r + colors[i].g + colors[i].b);
        }

        Debug.Log(lightLevel);
    }

    public void DeathLevel()
    {
        if (deathLevel <= 0)
        {
            deathLevel = 0;
        }
        else if (deathLevel >= maxDeathLevel)
        {
            Debug.Log("DED");
            deathLevel = maxDeathLevel;
        }

        if (lightLevel <= 20f)
        {
            deathLevel += Time.deltaTime;
        }
        else
        {
            deathLevel -= Time.deltaTime;
        }

        deathSlider.value = deathLevel;
    }
}
