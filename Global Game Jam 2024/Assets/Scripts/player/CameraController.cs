using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float camSpeed;

    private void Start()
    {
        playerPos = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        Vector3 camPos = transform.position;
        
        Vector3 desiredPosition = playerPos.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(camPos, desiredPosition, camSpeed * Time.deltaTime);
        
        transform.position = smoothedPosition;
    }
}
