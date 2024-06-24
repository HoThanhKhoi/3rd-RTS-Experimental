using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

[CreateAssetMenu(fileName = "NewInputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IRTSControlActions
{
    private PlayerInput playerInput;

    public Vector2 MousePosition { get; private set; }
    public Vector2 MoveDirection { get; private set; }
    public Vector2 ZoomValue { get; private set; }


    public event Action MouseMovementEvent;
    public event Action MouseLeftClickEvent;
    public event Action MouseRightClickEvent;
    public event Action<bool> MouseMiddleClickEvent;

    public event Action CameraMovementPressEvent;
    public event Action CameraZoomEvent;

    public event Action OnKeyRPress;

    #region Unity Methods
    private void OnEnable()
    {
        playerInput = new PlayerInput();
        if (playerInput == null) { playerInput = new PlayerInput(); }

        playerInput.RTSControl.SetCallbacks(this);
        playerInput.RTSControl.Enable();

    }

    private void OnDisable()
    {
        playerInput.RTSControl.Disable();
    }

    #endregion

    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseLeftClickEvent?.Invoke();
        }
    }

    public void OnMouseRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseLeftClickEvent?.Invoke();
        }
    }

    public void OnMouseMiddleClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseRightClickEvent?.Invoke();
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();

        MouseMovementEvent?.Invoke();
    }

    public void OnMouseScroll(InputAction.CallbackContext context)
    {
        ZoomValue = context.ReadValue<Vector2>();

        CameraZoomEvent?.Invoke();
    }

    public void OnMiddleMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseMiddleClickEvent?.Invoke(true);

        }
        else if (context.canceled)
        {

            MouseMiddleClickEvent?.Invoke(false);
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        OnKeyRPress?.Invoke();
    }
}
