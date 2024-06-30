using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    public class CardManager : MonoSingleton<CardManager>
    {
        private bool firstPick;
        private bool secondPick;

        private int firstPickIndex;
        private int secondPickIndex;

        private int turns;
        private int matches;

        private int timer;

        private bool gameOver;

        [SerializeField] private TextMeshProUGUI _turnsText;
        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _timerText;

        private List<GameObject> cards = new List<GameObject>();
        public List<Sprite> cardImages = new List<Sprite>();
        private List<Transform> cardTransforms = new List<Transform>();

        [SerializeField] private Transform _cardsSpawnedTrans;

        [SerializeField] private AudioClip _cardMatchClip;
        [SerializeField] private AudioClip _cardNoMatchClip;
        [SerializeField] private AudioClip _gameOverClip;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(4f);

            while (!gameOver)
            {
                yield return new WaitForSeconds(1f);
                timer++;
                _timerText.text = $"Timer: {timer}"; // Update the timer text
            }
        }

        public void PrepareCards()
        {
            InitiliazeStartingValues();
            CollectAllSpawnedCards();
            AddListenersToCards();
            GetCardsTransforms();
            ShufleCardsTransforms();
            SetFrontSideImagesOfCards();
            StartCoroutine(StartTimer());
        }

        private void InitiliazeStartingValues()
        {
            firstPick = false;
            secondPick = false;
            gameOver = false;
            turns = 0;
            matches = 0;
            timer = 0;
            _timerText.text = $"Timer: {timer}";
            _turnsText.text = $"Turns: {turns}";
            _matchesText.text = $"Matches: {matches}";
        }



        private void CollectAllSpawnedCards()
        {
            for (int i = 0; i < _cardsSpawnedTrans.childCount; i++)
            {
                cards.Add(_cardsSpawnedTrans.GetChild(i).gameObject);
                cards[i].name = "" + i;
            }
            Debug.Log("Number of cards: " + cards.Count);
        }

        private void AddListenersToCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<Button>().onClick.AddListener(() => PickACard());
            }
        }

        private void PickACard()
        {
            string cardName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            Debug.Log("Card picked: " + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            if (!firstPick)
            {
                firstPick = true;
                firstPickIndex = int.Parse(cardName);

            }
            else if (!secondPick)
            {
                secondPick = true;
                secondPickIndex = int.Parse(cardName);
                StartCoroutine(CheckMatch());
            }
        }

        IEnumerator CheckMatch()
        {
            turns++;
            _turnsText.text = $"Turns: {turns}";
            Debug.Log("Turns: " + turns);

            yield return new WaitForSeconds(0.5f);

            if (cards[firstPickIndex].GetComponent<Card>().frontSide == cards[secondPickIndex].GetComponent<Card>().frontSide)
            {
                Debug.Log("Match!");
                matches++;
                _matchesText.text = $"Matches: {matches}";
                cards[firstPickIndex].SetActive(false);
                cards[secondPickIndex].SetActive(false);
                _audioSource.PlayOneShot(_cardMatchClip);

                if (matches == cards.Count / 2)
                {
                    Debug.Log("Game Over!");
                    gameOver = true;
                    _audioSource.PlayOneShot(_gameOverClip);
                    UIManager.Instance.ShowLevelCompletedCanvas();
                    ScoreManager.Instance.CalculateScore(matches, turns, timer);
                    RemoveAllCards();
                }
            }
            else
            {
                Debug.Log("No match!");
                cards[firstPickIndex].GetComponent<CardFlipper>().FlipCard();
                cards[secondPickIndex].GetComponent<CardFlipper>().FlipCard();
                _audioSource.PlayOneShot(_cardNoMatchClip);
            }

            firstPick = false;
            secondPick = false;
        }

        public int GetTurns() { return turns; }            
        public int GetMatches() { return matches; }            
        public int GetTimer(){ return timer; }
        public int TotalCards() { return cards.Count; }

        private void GetCardsTransforms()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cardTransforms.Add(cards[i].transform);
            }
        }

        private void SetFrontSideImagesOfCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<Card>().frontSide = cardImages[i / 2];
            }
            Debug.Log("Number of card images: " + cards.Count / 2);
        }

        private void RemoveAllCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i]);
            }
            cards.Clear();
        }

        private void ShufleCardsTransforms()
        {
            // Create a list of indices to track which transforms have been swapped
            List<int> indices = new List<int>();
            for (int i = 0; i < cardTransforms.Count; i++)
            {
                indices.Add(i);
            }

            // Shuffle the indices list
            for (int i = 0; i < indices.Count; i++)
            {
                int randomIndex = Random.Range(i, indices.Count);
                int temp = indices[i];
                indices[i] = indices[randomIndex];
                indices[randomIndex] = temp;
            }

            // Create temporary storage for the original transforms
            Vector3[] originalPositions = new Vector3[cardTransforms.Count];
            Quaternion[] originalRotations = new Quaternion[cardTransforms.Count];
            Vector3[] originalScales = new Vector3[cardTransforms.Count];

            for (int i = 0; i < cardTransforms.Count; i++)
            {
                originalPositions[i] = cardTransforms[i].position;
                originalRotations[i] = cardTransforms[i].rotation;
                originalScales[i] = cardTransforms[i].localScale;
            }

            // Assign the new transforms based on the shuffled indices
            for (int i = 0; i < cardTransforms.Count; i++)
            {
                cardTransforms[i].position = originalPositions[indices[i]];
                cardTransforms[i].rotation = originalRotations[indices[i]];
                cardTransforms[i].localScale = originalScales[indices[i]];
            }

            // Clear the list of card transforms
            cardTransforms.Clear();
        }
    }
}