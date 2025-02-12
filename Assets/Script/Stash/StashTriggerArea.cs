using System.Security.Cryptography;
using UnityEngine;

namespace Script.Stash
{
    public class StashTriggerArea : MonoBehaviour
    {
        [SerializeField] private GameObject stashPanel;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Magnet"))
            {
                Destroy(stashPanel);
            }
        }
    }
}