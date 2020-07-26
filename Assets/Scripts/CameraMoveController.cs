using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    private Vector2 offset = new Vector2(0, 0);
    private float smooth = 1.0f;
    private float zoomTarget;
    private float zoomFactor = 8f;
    private float zoomLerpSpeed = 5;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        zoomTarget = mainCamera.orthographicSize;
    }


    void Update()
    {
        MouseZoom();
    }

    void FixedUpdate()
    {
        //Vector2 averagePos = Vector2.Lerp(transform.position, SwarmAI.AveragePosition() + offset, Time.fixedDeltaTime * smooth);
        Vector2 averagePos = SwarmAI.AveragePosition();
        transform.position = new Vector3(averagePos.x, averagePos.y, -10);
    }


    public void MouseZoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        zoomTarget -= scrollData * zoomFactor;
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomTarget, Time.deltaTime * zoomLerpSpeed);
    }
}
