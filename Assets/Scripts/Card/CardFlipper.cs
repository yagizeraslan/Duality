using System.Collections;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class CardFlipper : MonoBehaviour
    {
        private float _flipDuration = 0.25f;
        private bool _isFlipping = false;
        private bool _isShowingFront = false;
        private Card _card;
        private CardSoundPlayer _soundPlayer;

        private void Awake()
        {
            _card = GetComponent<Card>();
            _soundPlayer = GetComponent<CardSoundPlayer>();
        }

        public void FlipCard()
        {
            if (_isFlipping)
                return;

            StartCoroutine(Flip());
        }

        private IEnumerator Flip()
        {
            _isFlipping = true;
            _soundPlayer.PlayFlipSound();

            yield return ScaleOverTime(1f, 0f, _flipDuration / 2);

            _card.SetSprite(!_isShowingFront ? _card.FrontSide : _card.BackSide);

            yield return ScaleOverTime(0f, 1f, _flipDuration / 2);

            _isShowingFront = !_isShowingFront;
            _isFlipping = false;
        }

        private IEnumerator ScaleOverTime(float from, float to, float duration)
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float scale = Mathf.Lerp(from, to, time / duration);
                transform.localScale = new Vector3(scale, 1f, 1f);
                yield return null;
            }
        }
    }
}