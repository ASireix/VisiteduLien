using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressInputBase : MonoBehaviour
{
    protected InputAction _pressedAction;
    protected InputAction _axisAction;

    private void Awake()
    {
        _pressedAction = new InputAction("touch", binding: "<Pointer>/press");
        _axisAction = new InputAction("axis", binding: "<Pointer>/delta");

        _pressedAction.started += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPressBegan(device.position.ReadValue());
            }
        };

        _pressedAction.performed += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPress(device.position.ReadValue());
            }
        };

        _pressedAction.canceled += _ => OnPressCancel();
    }

    protected virtual void OnPressBegan(Vector3 position) { }
    protected virtual void OnPress(Vector3 position) { }
    protected virtual void OnPressCancel() { }

    protected virtual void OnEnable()
    {
        _pressedAction.Enable();
        _axisAction.Enable();
    }

    protected virtual void OnDisable()
    {
        _pressedAction.Disable();
        _axisAction.Disable();
    }

    protected virtual void OnDestroy()
    {
        _pressedAction.Dispose();
        _axisAction.Dispose();
    }
}
