using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Notepad : MonoBehaviour
{
	public InputActionProperty toggleOffButton; // Assign in Inspector
	public InputActionProperty toggleOnButton;  // Assign in Inspector

	private List<Transform> children = new List<Transform>(); // Store child objects
	private Dictionary<Transform, bool> childStates = new Dictionary<Transform, bool>(); // Track state per child
	private Dictionary<Transform, bool> childUnlocked = new Dictionary<Transform, bool>(); // Track state per child

	private int disableIndex = 0; // Tracks which child is being disabled next
	private int enableIndex = 0;  // Tracks which child is being enabled next

	[HideInInspector]
	public int highestIndex = 0;

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
	}

	void Update()
	{
		bool isPressed = primaryButtonAction.action.ReadValue<float>() == 1f;

		// Detect the transition from "not pressed" to "pressed"
		if (isPressed && !wasPressed)
		{
			isNotebookOpen = !isNotebookOpen; // Toggle state
			Debug.Log($"Primary button (A/X) pressed! isNotebookOpen: {isNotebookOpen}");
            UpdateChildPositions();
        }
		if (isNotebookOpen == true)
        {
            gameObject.transform.position = mainCam.transform.position + (mainCam.transform.forward / 3);
			gameObject.transform.LookAt((mainCam.transform));
			gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
			if (toggleOffButton.action.WasPressedThisFrame())
			{
				while (disableIndex < children.Count)
				{
					Transform child = children[disableIndex];

					if (childStates[child]) // Only disable active ones
					{
						child.gameObject.SetActive(false);
                        if (disableIndex > 0)
                        {
                            disableIndex--;
                        }
                        if (enableIndex > 0)
						{
							enableIndex--;
						}
						print("disableIndex " + disableIndex);
						print("enableIndex " + enableIndex);
						break;
						 // Stop after disabling one
					}
					if (disableIndex > children.Count)
					{
						break;
					}
                    if (disableIndex > 0)
                    {
                        disableIndex--;
                    } // Skip if already inactive
                    if (enableIndex > 0)
					{
						enableIndex--;
					}
					print("disableIndex " + disableIndex);
					print("enableIndex " + enableIndex);
					//disableIndex++; // Skip if already inactive
				}
			}

			// Toggle ON: Enable one inactive child at a time
			if (toggleOnButton.action.WasPressedThisFrame())
			{
				while (enableIndex < children.Count)
				{
					Transform child = children[enableIndex];
					if (childUnlocked[child] == true)
					{
						child.gameObject.SetActive(true);
						childStates[child] = true;
						enableIndex++;
                        if (disableIndex < children.Count - 1)
                        {
                            disableIndex++;
                        }
                        print("disableIndex " + disableIndex);
						print("enableIndex " + enableIndex);
						break;
					}
					if (enableIndex > children.Count)
					{
						break;
					}
					if (disableIndex < children.Count - 1)
                    {
                        disableIndex++;
                    }
					enableIndex++; // Skip if already active
					print("disableIndex " + disableIndex);
					print("enableIndex " + enableIndex);

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
				children[i].position += new Vector3(0,0.0002f,0);
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

        if (childIndex > highestIndex)
        {
            disableIndex = childIndex;
        }

        highestIndex = children
		.Select((child, index) => new { Index = index, IsActive = childStates[child] })
		.Where(item => item.IsActive)
		.Select(item => item.Index)
		.DefaultIfEmpty(-1)
		.Max();

		foreach (Transform child in transform)
		{
			childStates[child] = childUnlocked[child];
			child.gameObject.SetActive(childStates[child]);
		}
		print("disable index is" + disableIndex);
        print("highest index is" + highestIndex);
    }
	
}
