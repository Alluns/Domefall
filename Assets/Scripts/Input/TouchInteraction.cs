using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

namespace Input
{
    public class TouchInteraction : MonoBehaviour
    {
        public static TouchInteraction Instance;

        public GameObject selectedObject;
        
        private InputAction tapAction;
        private Ray ray;
    
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                throw new Exception("2 singleton of the same [TouchInteraction exist]");
            }
            Instance = this;
            
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

            if (selectedObject != null && selectedObject != hit.collider.gameObject)
            {
                selectedObject?.GetComponent<IClickable>()?.DeSelected();
                hit.collider.gameObject.GetComponent<IClickable>()?.Selected();
            }
            
            selectedObject = hit.collider.gameObject;
            
            selectedObject.GetComponent<IClickable>()?.Clicked();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.rebeccaPurple;
            Gizmos.DrawRay(ray.origin, ray.direction * 100f);
        }
    }
}
