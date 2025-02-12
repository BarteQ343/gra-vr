using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
	public Unlocker keyToDoor;
	public Animator anim;
	public bool unlockedByDefault = false;
	public AudioSource audioSourceUnlocked;
    public AudioSource audioSourceLocked;

    public void OpenDoor()
	{
		if (keyToDoor == null && unlockedByDefault == true)
		{
			if (anim.GetBool("open") == false)
			{
				print("opening");
				anim.SetBool("open", true);
				audioSourceUnlocked.Play();
			}
			else
			{
				print("closing");
				anim.SetBool("open", false);
                audioSourceUnlocked.Play();
			}
		}
		else if (keyToDoor.unlocked == true)
		{
			if (anim.GetBool("open") == false)
			{
				print("opening");
				anim.SetBool("open", true);
                audioSourceUnlocked.Play();
			}
			else
			{
				print("closing");
				anim.SetBool("open", false);
                audioSourceUnlocked.Play();
			}
		}
		else if (keyToDoor.unlocked == false)
		{
			audioSourceLocked.Play();
		}
	}
}
