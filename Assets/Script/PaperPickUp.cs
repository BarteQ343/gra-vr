using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaperPickUp : MonoBehaviour
{
	[SerializeField] GameObject mainCam;
	Transform mainCamTransform;
	bool followingCam = false;
	public InputActionProperty triggerAction; // Assign in Inspector
	bool pickedUp = false;
	public Notepad notepad;
	[SerializeField] int noteToUnlock;
    public void PickUp()
	{
		followingCam = true;
	}
	private void Update()
	{
		if (followingCam == true)
		{
            notepad.GetComponent<Notepad>().SetChildState(noteToUnlock, true); // Activates 3rd child
            gameObject.transform.position = mainCam.transform.position + (mainCam.transform.forward / 3);
			gameObject.transform.LookAt((mainCam.transform));
			gameObject.transform.rotation *= Quaternion.Euler(90, 0, 0);
			pickedUp = true;
		}
		if (triggerAction.action.ReadValue<float>() > 0.1f && pickedUp) // Works for both press & analog triggers
		{
			Destroy(gameObject);
		}
	}
}
