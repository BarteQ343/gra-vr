
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public bool unlocked = false;

    public void UnlockDoor()
    {
        print(unlocked);
        gameObject.SetActive(false);
        unlocked = true;
    }
}
