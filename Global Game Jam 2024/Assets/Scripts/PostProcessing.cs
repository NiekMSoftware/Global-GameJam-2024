using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PostProcessing : MonoBehaviour
{
    public float intensity = 1.0f; // Example intensity value
    Volume _volume;
    Vignette _vignette = null;

    void Start()
    {
        _volume = GetComponent<Volume>();

        if (!_volume)
        {
            _volume = gameObject.AddComponent<Volume>();
            _volume.isGlobal = true; // Assuming you want the effect to be global
        }


    }

    void Update()
    {
        // Check if the Vignette component is not null before modifying its values

        _volume.profile.TryGet(out Vignette vignette);
        float intensity = (float)vignette.intensity;

    }
}
