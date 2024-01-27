using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PostProcessing : MonoBehaviour
{
    public float defaultIntensity = 0.0f;
    public float endIntensity = 0.3f;
    public float duration = 2.0f; // Change this to the desired duration

    Volume _volume;
    Vignette _vignette = null;

    private float elapsedTime = 0.0f;
    private bool isForward = true; // Flag to track the direction of interpolation

    void Start()
    {
        _volume = GetComponent<Volume>();

        if (!_volume)
        {
            _volume = gameObject.AddComponent<Volume>();
            _volume.isGlobal = true; // Assuming you want the effect to be global
        }

        // Initialize the Vignette component
        _volume.profile.TryGet(out _vignette);
    }

    void Update()
    {
        // Check if the Vignette component is not null before modifying its values
        if (_vignette != null)
        {
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor between 0 and 1 based on the elapsed time and duration
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate the intensity value
            float newIntensity;

            if (isForward)
                newIntensity = Mathf.Lerp(defaultIntensity, endIntensity, t);
            else
                newIntensity = Mathf.Lerp(endIntensity, defaultIntensity, t);

            // Set the Vignette intensity
            _vignette.intensity.value = newIntensity;

            // Optionally, you can perform additional actions based on the interpolated intensity value
            Debug.Log("Current Intensity: " + newIntensity);

            // Check if the interpolation is complete
            if (elapsedTime >= duration)
            {
                Debug.Log("Tween Complete!");

                // Reverse the direction for the next iteration
                isForward = !isForward;

                // Reset elapsed time for looping or other actions
                elapsedTime = 0.0f;
            }
        }
    }
}
