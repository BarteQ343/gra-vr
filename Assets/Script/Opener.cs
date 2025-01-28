using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
    public Unlocker keyToDoor;
    public Animator anim;

    public void OpenDoor()
    {
        if (keyToDoor.unlocked == true)
        {
            if (anim.GetBool("open") == false)
            {
                print("opening");
                anim.SetBool("open", true);
            }
            else
            {
                print("closing");
                anim.SetBool("open", false);
            }
        }
    }
}
