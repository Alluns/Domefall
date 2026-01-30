using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Input
{
    public class TouchInteraction : MonoBehaviour
    {
        private InputAction tapAction;

        private GameObject selectedObject;
        private Ray ray;
    
        private void Awake()
        {
            tapAction = InputSystem.actions.FindAction("TouchInput");
        }

        private void OnEnable()
        {
            tapAction.Enable();
            tapAction.performed += TouchPerformed;
        }
    
        private void OnDisable()
        {
            tapAction.Disable();
            tapAction.performed -= TouchPerformed;
        }

        private void TouchPerformed(InputAction.CallbackContext context)
        {
            TouchState touch = context.ReadValue<TouchState>();
        
            if (!touch.isTap) return;

            // Check if we are clicking on a UI element
        
            PointerEventData pointerEventData = new(EventSystem.current)
            {
                position = touch.position
            };

            List<RaycastResult> results = new();

            EventSystem.current.RaycastAll(pointerEventData, results);

            if (results.Count > 0)
            {
                return;
            }
        
            // Finished UI check

            Vector2 pressedPixel = touch.startPosition;

            if (!Camera.main) return;
        
            ray = Camera.main.ScreenPointToRay(pressedPixel);

            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            
            hit.collider.gameObject.GetComponent<IClickable>()?.Clicked();

            if (selectedObject == hit.collider.gameObject) return;
        
            selectedObject?.GetComponent<IClickable>()?.DeSelected();
        
            selectedObject = hit.collider.gameObject;
        
            selectedObject.GetComponent<IClickable>()?.Selected();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.rebeccaPurple;
            Gizmos.DrawRay(ray.origin, ray.direction * 100f);
        }
    }
}
