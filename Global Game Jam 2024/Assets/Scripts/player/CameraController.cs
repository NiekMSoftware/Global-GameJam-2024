using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerPos;
    private Vector3 desiredPosition;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float camSpeed;
    
    private void Start()
    {
        playerPos = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        desiredPosition = playerPos.position + offset;
    }

    private void LateUpdate()
    {
        Vector3 camPos = transform.position;
    
        float blendFraction = 1f - Mathf.Exp(-camSpeed * Time.deltaTime);
        Vector3 smoothedPosition = Vector3.Lerp(camPos, desiredPosition, blendFraction);
        
        transform.position = smoothedPosition;
    }
}
