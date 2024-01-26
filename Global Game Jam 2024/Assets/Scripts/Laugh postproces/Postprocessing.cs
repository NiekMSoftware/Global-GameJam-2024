using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class Postprocessing : MonoBehaviour
{
    public PostProcessProfile postProcessProfile;

    [Range(0f, 1f)] // Restrict the slider range between 0 and 1
    public float vignetteIntensity = 1f; // Default intensity value

    private Vignette vignetteLayer;

    void Start()
    {
        if (postProcessProfile == null)
        {
            Debug.LogError("Post Process Profile is not assigned!");
            return;
        }

        vignetteLayer = postProcessProfile.GetSetting<Vignette>();
        if (vignetteLayer == null)
        {
            Debug.LogError("Vignette effect is not found in the Post Process Profile!");
            return;
        }


        // Set the initial vignette intensity
        UpdateVignetteIntensity(vignetteIntensity);
    }

    void UpdateVignetteIntensity(float value)
    {
        Debug.Log("Vignette intensity updated: " + value);

        if (vignetteLayer != null)
        {
            vignetteLayer.intensity.value = value;
        }
    }
}
