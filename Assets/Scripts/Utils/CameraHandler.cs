using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public enum CameraMovementPriorty
{
    WASD,
    Edge,
    Pan
}

public class CameraHandler : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Data")]
    [SerializeField] private float screenBorderSize = 10f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float minOrthographicSize = 5f;
    [SerializeField] private float maxOrthographicSize = 15f;
    [Header("Map Bounds")]
    [SerializeField] private float mapMinX = -1f;
    [SerializeField] private float mapMaxX = 1f;
    [SerializeField] private float mapMinY = -1f;
    [SerializeField] private float mapMaxY = 1f;
    [Header("Camera Bounds")]
    [SerializeField] private float minX = -1f;
    [SerializeField] private float maxX = 1f;
    [SerializeField] private float minY = -1f;
    [SerializeField] private float maxY = 1f;


    private float orthographicSize;
    private float targetOrthographicSize;

    private Vector3 targetPosition;

    private Vector3 moveDir;
    private Vector3 moveDirForEdge;
    private Vector3 moveDirForPan;

    private Vector2 mousePositionForEdge;
    private Vector2 mousePositionForPan;
    private Vector2 middleMouseClickPosition;

    private bool middleMouseButtonPressed = false;

    private CameraMovementPriorty cameraMovementPriority;

    #region Unity Methods
    void Start()
    {
        cameraMovementPriority = CameraMovementPriorty.Edge;

        inputReader.CameraZoomEvent += OnCameraZoom;

        inputReader.MouseMiddleClickEvent += MiddleMouseClick;
        inputReader.MouseMovementEvent += MoveCameraWithMouse;


        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;

        ClampCameraMovement();
        ClampCameraMovementSpeed();
    }

    private void Update()
    {
        mousePositionForEdge = inputReader.MousePosition;
        mousePositionForPan = inputReader.MousePosition;

        OnCameraZoom();

        if (cameraMovementPriority == CameraMovementPriorty.Edge)
        {
            MoveCameraWithMouse();
        }
        else if (cameraMovementPriority == CameraMovementPriorty.Pan)
        {
            CameraPan();
        }


        //Debug.Log("move direction: " + moveDir);
        //Debug.Log("middle mouse CLICK position: " + middleMouseClickPosition);
        //Debug.Log("mouse position for EDGE: " + mousePositionForEdge);
        //Debug.Log("mouse position: " + mousePositionForPan);

        transform.position = targetPosition;
    }

    #endregion

    private void ClampCameraMovement()
    {
        float clampX = Mathf.Clamp(targetPosition.x, mapMinX, mapMaxX);
        float clampY = Mathf.Clamp(targetPosition.y, mapMinY, mapMaxY);
        targetPosition = new Vector3(clampX, clampY, targetPosition.z);
    }

    private void ClampCameraMovementSpeed()
    {
        float clampX = Mathf.Clamp(moveDir.x, minX, maxX);
        float clampY = Mathf.Clamp(moveDir.y, minY, maxY);
        moveDir = new Vector3(clampX, clampY, moveDir.z);
    }

    private void OnCameraZoom()
    {

        targetOrthographicSize -= inputReader.ZoomValue.y * zoomAmount;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime * 20f);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    private void UpdateCameraMovement()
    {
        targetPosition += moveDir.normalized * moveSpeed * Time.deltaTime;
    }

    private void MoveCameraByEdge()
    {
        //Horizontal Mouse
        if (mousePositionForEdge.x <= screenBorderSize)
        {
            moveDirForEdge.x = Mathf.Lerp(moveDirForEdge.x, -1f, scrollSpeed);
        }
        else if (mousePositionForEdge.x >= Screen.width - screenBorderSize)
        {
            moveDirForEdge.x = Mathf.Lerp(moveDirForEdge.x, 1f, scrollSpeed);
        }
        else if (screenBorderSize < mousePositionForEdge.x && mousePositionForEdge.x < Screen.width - screenBorderSize)
        {
            moveDirForEdge.x = 0;
        }


        //Vertical Mouse
        if (mousePositionForEdge.y <= screenBorderSize)
        {
            moveDirForEdge.y = Mathf.Lerp(moveDirForEdge.y, -1f, scrollSpeed);
        }
        else if (mousePositionForEdge.y >= Screen.height - screenBorderSize)
        {
            moveDirForEdge.y = Mathf.Lerp(moveDirForEdge.y, 1f, scrollSpeed);
        }
        else if (screenBorderSize < mousePositionForEdge.y && mousePositionForEdge.y < Screen.height - screenBorderSize)
        {
            moveDirForEdge.y = 0;
        }


        moveDir = moveDirForEdge;

        UpdateCameraMovement();
    }

    private void MoveCameraWithMouse()
    {


        //Horizontal Mouse
        if (mousePositionForEdge.x <= screenBorderSize || mousePositionForEdge.x >= Screen.width - screenBorderSize)
        {
            cameraMovementPriority = CameraMovementPriorty.Edge;
            MoveCameraByEdge();
        }
        else if (screenBorderSize < mousePositionForEdge.x && mousePositionForEdge.x < Screen.width - screenBorderSize)
        {
            moveDirForEdge.x = 0;
        }


        //Vertical Mouse
        if (mousePositionForEdge.y <= screenBorderSize || mousePositionForEdge.y >= Screen.height - screenBorderSize)
        {
            cameraMovementPriority = CameraMovementPriorty.Edge;
            MoveCameraByEdge();
        }
        else if (screenBorderSize < mousePositionForEdge.y && mousePositionForEdge.y < Screen.height - screenBorderSize)
        {
            moveDirForEdge.y = 0;
        }

    }

    private void MiddleMouseClick(bool obj)
    {
        middleMouseButtonPressed = obj;
        middleMouseClickPosition = mousePositionForPan;

        if (obj == true)
        {
            cameraMovementPriority = CameraMovementPriorty.Pan;
        }
    }

    private void CameraPan()
    {

        if (cameraMovementPriority == CameraMovementPriorty.Pan)
        {
            if (!middleMouseButtonPressed)
            {
                moveDirForPan = Vector3.zero;
                middleMouseButtonPressed = false;
                middleMouseClickPosition = Vector2.zero;
            }
            else if (middleMouseButtonPressed)
            {
                moveDirForPan = mousePositionForPan - middleMouseClickPosition;
            }
        }

        moveDir = moveDirForPan;


        UpdateCameraMovement();
    }
}
