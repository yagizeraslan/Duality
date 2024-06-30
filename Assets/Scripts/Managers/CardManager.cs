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
        private List<int> inactiveCards = new List<int>();
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
                PlayerPrefs.SetInt("Timer", timer);
                _timerText.text = $"Timer: {timer}"; // Update the timer text
            }
        }

        #region New Game Selection Functions
        
        // New game selection
        public void CreateCards()
        {
            InitiliazeStartingValues();
            CollectAllSpawnedCards();
            AddListenersToCards();
            GetCardsTransforms();
            ShufleCardsTransforms();
            SetFrontSideImagesOfCards();
            StartCoroutine(StartTimer());
        }

        // Initialize the starting values of the game
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

        #endregion

        #region Resume Game Selection Functions
        // Resume game selection
        public void LoadPreviousCards()
        {
            LoadInitializeStartingValues();
            CollectAllSpawnedCards();
            AddListenersToCards();
            GetCardsTransforms();
            LoadCardsdShuffledTransforms();
            SetFrontSideImagesOfCards();
            SetMatchedCardsInactivities();
            StartCoroutine(StartTimer());
        }

        public void LoadInitializeStartingValues()
        {
            firstPick = false;
            secondPick = false;
            gameOver = false;
            turns = PlayerPrefs.GetInt("Turns");
            matches = PlayerPrefs.GetInt("Matches");
            timer = PlayerPrefs.GetInt("Timer");
            _timerText.text = $"Timer: {timer}";
            _turnsText.text = $"Turns: {turns}";
            _matchesText.text = $"Matches: {matches}";
        }
        #endregion

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
            PlayerPrefs.SetInt("Turns", turns);
            _turnsText.text = $"Turns: {turns}";
            Debug.Log("Turns: " + turns);

            yield return new WaitForSeconds(0.5f);

            if (cards[firstPickIndex].GetComponent<Card>().frontSide == cards[secondPickIndex].GetComponent<Card>().frontSide)
            {
                Debug.Log("Match!");
                matches++;
                _matchesText.text = $"Matches: {matches}";
                PlayerPrefs.SetInt("Matches", matches);

                SaveMatchedCard(firstPickIndex);
                SaveMatchedCard(secondPickIndex);

                if (cards.Count / 2 == matches)
                {
                    cards[firstPickIndex].GetComponent<Button>().interactable = false;
                    cards[secondPickIndex].GetComponent<Button>().interactable = false;
                    StartCoroutine(FadeOutCard(cards[firstPickIndex].GetComponent<Image>(), 0f));
                    StartCoroutine(FadeOutCard(cards[secondPickIndex].GetComponent<Image>(), 0f));
                }
                else
                {
                    cards[firstPickIndex].GetComponent<Button>().interactable = false;
                    cards[secondPickIndex].GetComponent<Button>().interactable = false;
                    StartCoroutine(FadeOutCard(cards[firstPickIndex].GetComponent<Image>(), 0.5f));
                    StartCoroutine(FadeOutCard(cards[secondPickIndex].GetComponent<Image>(), 0.5f));
                }

                _audioSource.PlayOneShot(_cardMatchClip);

                if (matches == cards.Count / 2)
                {
                    Debug.Log("Game Over!");
                    gameOver = true;
                    _audioSource.PlayOneShot(_gameOverClip);
                    ScoreManager.Instance.CalculateScore(matches, turns, timer);
                    ProgressManager.Instance.ClearProgress();
                    RemoveAllCards();
                    UIManager.Instance.ShowLevelCompletedCanvas();
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
        public int GetTimer() { return timer; }
        public int TotalCards() { return cards.Count; }

        // Fade out the card image when a match is found
        private IEnumerator FadeOutCard(Image cardImage, float duration)
        {
            float elapsedTime = 0f;
            Color startColor = cardImage.color;
            Color endColor = new Color(1, 1, 1, 0);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                cardImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                yield return null;
            }

            cardImage.color = endColor;
        }

        // Get the transforms of the cards
        private void GetCardsTransforms()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cardTransforms.Add(cards[i].transform);
            }
        }

        // Set the front side images of the cards
        private void SetFrontSideImagesOfCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].GetComponent<Card>().frontSide = cardImages[i / 2];
            }
            Debug.Log("Number of card images: " + cards.Count / 2);
        }

        // Remove all cards from the scene
        private void RemoveAllCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i]);
            }
            cards.Clear();
        }

        // Shuffle the transforms of the cards
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

            SaveCardsShuffledTransforms();

            // Clear the list of card transforms
            cardTransforms.Clear();
        }

        private void SaveCardsShuffledTransforms()
        {
            for (int i = 0; i < cardTransforms.Count; i++)
            {
                PlayerPrefs.SetString($"Card_{i}_AnchoredPosition_{i}", SerializeVector2(cardTransforms[i].GetComponent<RectTransform>().anchoredPosition));
            }
            PlayerPrefs.SetInt("CardCount", cardTransforms.Count);
            Debug.Log("Card count: " + cardTransforms.Count);
            PlayerPrefs.Save();
        }

        private void LoadCardsdShuffledTransforms()
        {
            int count = PlayerPrefs.GetInt("CardCount");
            for (int i = 0; i < count; i++)
            {
                cardTransforms[i].GetComponent<RectTransform>().anchoredPosition = DeserializeVector2(PlayerPrefs.GetString($"Card_{i}_AnchoredPosition_{i}"));
            }
        }

        public void ReadShuffledTransforms()
        {
            int count = PlayerPrefs.GetInt("CardCount");
            for (int i = 0; i < count; i++)
            {
                string vectorString = PlayerPrefs.GetString($"Card_{i}_AnchoredPosition_{i}");
                Debug.Log($"Card_{i}_AnchoredPosition_{i}: {vectorString}");
            }
        }

        #region Serialization Functions
        private string SerializeVector2(Vector2 vector)
        {
            return vector.x + "," + vector.y;
        }

        private Vector2 DeserializeVector2(string vectorString)
        {
            string[] values = vectorString.Split(',');
            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }
        #endregion

        // Save matched card index
        private void SaveMatchedCard(int cardIndex)
        {
            string matchedCards = PlayerPrefs.GetString("MatchedCards", "");
            matchedCards += cardIndex.ToString() + ",";
            PlayerPrefs.SetString("MatchedCards", matchedCards);
        }

        // Set matched cards as inactive based on saved data
        private void SetMatchedCardsInactivities()
        {
            string matchedCards = PlayerPrefs.GetString("MatchedCards", "");
            if (!string.IsNullOrEmpty(matchedCards))
            {
                string[] matchedCardIndices = matchedCards.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string cardIndexStr in matchedCardIndices)
                {
                    int cardIndex = int.Parse(cardIndexStr);
                    cards[cardIndex].GetComponent<Button>().interactable = false;
                    cards[cardIndex].GetComponent<Image>().color = new Color(1, 1, 1, 0); // Hide the card image
                }
            }
        }
    }
}