using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Notepad : MonoBehaviour
{
	public InputActionProperty toggleOffButton; // Assign in Inspector
	public InputActionProperty toggleOnButton;  // Assign in Inspector

	private List<Transform> children = new List<Transform>(); // Store child objects
	private Dictionary<Transform, bool> childStates = new Dictionary<Transform, bool>(); // Track state per child
    private Dictionary<Transform, bool> childUnlocked = new Dictionary<Transform, bool>(); // Track state per child
    private Vector3[] childOffsets;

    private int disableIndex = 0; // Tracks which child is being disabled next
	private int enableIndex = 0;  // Tracks which child is being enabled next


	public InputActionProperty primaryButtonAction;
	bool isNotebookOpen = false;
	private bool wasPressed = false; // Tracks previous button state
	[SerializeField] GameObject mainCam;

	void Start()
	{
		// Store all children in the list and set default states (each child is manually set)
		foreach (Transform child in transform)
		{
			children.Add(child);
			childStates[child] = false; // Default: All start inactive (adjust as needed)
            childUnlocked[child] = false;
			child.gameObject.SetActive(childStates[child]);
		}
        childOffsets = new Vector3[children.Count];
        for (int i = 0; i < children.Count; i++)
        {
            childOffsets[i] = children[i].localPosition; // Save initial local positions
        }
    }

	void Update()
	{
		bool isPressed = primaryButtonAction.action.ReadValue<float>() == 1f;

		// Detect the transition from "not pressed" to "pressed"
		if (isPressed && !wasPressed)
		{
			isNotebookOpen = !isNotebookOpen; // Toggle state
			Debug.Log($"Primary button (A/X) pressed! isNotebookOpen: {isNotebookOpen}");
		}
		if (isNotebookOpen == true)
		{
			gameObject.transform.position = mainCam.transform.position + (mainCam.transform.forward / 3);
			gameObject.transform.LookAt((mainCam.transform));
			gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
            UpdateChildPositions();
            if (toggleOffButton.action.WasPressedThisFrame())
            {
                print("toggleoff");
                while (disableIndex < children.Count)
                {
                    Transform child = children[disableIndex];
                    if (childStates[child]) // Only disable active ones
                    {
                        child.gameObject.SetActive(false);
                        disableIndex++;
                        break; // Stop after disabling one
                    }
                    disableIndex++; // Skip if already inactive
                }
            }

            // Toggle ON: Enable one inactive child at a time
            if (toggleOnButton.action.WasPressedThisFrame())
            {
                print("toggleon");
                while (enableIndex < children.Count)
                {
                    Transform child = children[enableIndex];
                    if (!childStates[child]) // Only enable inactive ones
                    {
                        if (childUnlocked[child] == true)
                        {
                            child.gameObject.SetActive(true);
                            childStates[child] = true;
                            enableIndex++;
                            break;
                        }
                    }
                    enableIndex++; // Skip if already active
                }
            }
        }
		if (isNotebookOpen == false)
        {
            gameObject.transform.position = Vector3.zero;
        }
		wasPressed = isPressed; // Update last frame state
	}
    void UpdateChildPositions()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (childStates[children[i]]) // Only move active children
            {
                children[i].position = transform.position + transform.rotation * childOffsets[i];
            }
        }
    }

    // External method to manually set child states (e.g., picking up objects)
    public void SetChildState(int childIndex, bool isActive)
	{
        if (childIndex >= 0 && childIndex < children.Count)
        {
            Transform child = children[childIndex];
            childStates[child] = isActive;
            child.gameObject.SetActive(isActive);
            childUnlocked[child] = true;
        }
    }
	
}
