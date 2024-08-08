using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
[CreateAssetMenu(fileName = "InputReader", menuName = "Game/InputReader")]
public class InputReader : ScriptableObject, Controls.IMapActions
{
    public event UnityAction<float> pinchEvent;

    Controls controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Map.SetCallbacks(this);
        }

        if (!EnhancedTouchSupport.enabled)
        {
            EnhancedTouchSupport.Enable();
        }

        EnablePlayerInput();
    }


    private void OnDisable()
    {
        DisableAllInput();
    }


    public void OnPinch(InputAction.CallbackContext context)
    {
        if (pinchEvent != null)
        {
            // if there are not two active touches, return
            if (Touch.activeTouches.Count < 2)
                return;

            // get the finger inputs
            Touch primary = Touch.activeTouches[0];
            Touch secondary = Touch.activeTouches[1];

            // check if none of the fingers moved, return
            if (primary.phase == TouchPhase.Moved || secondary.phase == TouchPhase.Moved)
            {
                // if fingers have no history, then return
                if (primary.history.Count < 1 || secondary.history.Count < 1)
                    return;

                // calculate distance before and after touch movement
                float currentDistance = Vector2.Distance(primary.screenPosition, secondary.screenPosition);
                float previousDistance = Vector2.Distance(primary.history[0].screenPosition, secondary.history[0].screenPosition);

                // the zoom distance is the difference between the previous distance and the current distance
                float pinchDistance = currentDistance - previousDistance;
                pinchEvent.Invoke(pinchDistance);
            }
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (pinchEvent != null && !context.performed)
        {
            pinchEvent.Invoke(context.ReadValue<Vector2>().y);
        }
    }

    public void EnablePlayerInput()
    {
        controls.Map.Enable();
    }

    public void DisableAllInput()
    {
        controls.Map.Disable();
    }

    
}