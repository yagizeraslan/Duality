using UnityEngine;

namespace YagizEraslan.Duality
{
    public class CardSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _flipSound;

        public void PlayFlipSound()
        {
            PlaySound(_flipSound);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }
        }
    }
}