using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    public class CardInitializer : MonoBehaviour
    {
        private float _showAtBeginning = 2f;
        private Button _button;
        private Card _card;
        private CardFlipper _cardFlipper;

        private void Awake()
        {
            _card = GetComponent<Card>();
            _cardFlipper = GetComponent<CardFlipper>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _button.interactable = false;
            _button.onClick.AddListener(_cardFlipper.FlipCard);

            _card.SetSprite(_card.BackSide);

            StartCoroutine(ShowAtBeginning());
        }

        private IEnumerator ShowAtBeginning()
        {
            yield return new WaitForSeconds(_showAtBeginning);
            _cardFlipper.FlipCard();
            yield return new WaitForSeconds(_showAtBeginning);
            _cardFlipper.FlipCard();
            _button.interactable = true;
        }
    }
}
