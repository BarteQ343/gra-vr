using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Script.Flashlight
{
    public class FlashlightToggle : MonoBehaviour
    {
        [SerializeField] private GameObject flashlight;
        
        private XRGrabInteractable grabInteractable;
        private XRIDefaultInputActions controllerAction;
        private bool isHeld = false;
        private bool isOn = false;

        private void Awake()
        {
            controllerAction = new XRIDefaultInputActions();
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
            controllerAction.XRILeftInteraction.Enable();
            controllerAction.XRIRightInteraction.Enable();
            
            controllerAction.XRIRightInteraction.Activate.performed += ToggleLight;
            controllerAction.XRILeftInteraction.Activate.performed += ToggleLight;
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
            controllerAction.XRILeftInteraction.Disable();
            controllerAction.XRIRightInteraction.Disable();
            
            controllerAction.XRIRightInteraction.Activate.performed -= ToggleLight;
            controllerAction.XRILeftInteraction.Activate.performed -= ToggleLight;
        }

        private void OnGrab(SelectEnterEventArgs args)
        {
            isHeld = true;
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            isHeld = false;
        }

        private void ToggleLight(InputAction.CallbackContext obj)
        {
            if (isHeld)
            {
                Debug.Log("Toggling flashlight");
                flashlight.SetActive(isOn);
                isOn = !isOn;
            }
        }
    }
}
