using UnityEngine;

namespace Script.Stash
{
    public class StashTriggerArea : MonoBehaviour
    {
        public Animator anim;
        public AudioSource audioSourceUnlocked;
        [SerializeField] private GameObject stashPanel;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Magnet"))
            {
                anim.SetBool("open", true);
                audioSourceUnlocked.Play();
            }
        }
    }
}