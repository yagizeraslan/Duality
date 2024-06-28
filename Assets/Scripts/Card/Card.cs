using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    [RequireComponent(typeof(CardInitializer))]
    [RequireComponent(typeof(CardFlipper))]
    [RequireComponent(typeof(CardSoundPlayer))]
    public class Card : MonoBehaviour
    {
        [Header("Image References")]
        [SerializeField] private Sprite _backSide;
        [SerializeField] private Sprite _frontSide;

        private Image _cardImage;

        public Sprite BackSide => _backSide;
        public Sprite FrontSide => _frontSide;
        public Image CardImage => _cardImage;

        private void Awake()
        {
            _cardImage = GetComponent<Image>();
        }

        public void SetSprite(Sprite sprite)
        {
            _cardImage.sprite = sprite;
        }
    }
}