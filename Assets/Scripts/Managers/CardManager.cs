using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class CardManager : MonoSingleton<CardManager>
    {
        private List<GameObject> cards = new List<GameObject>();
        public List<Sprite> cardImages = new List<Sprite>();
        private List<Transform> cardTransforms = new List<Transform>();

        public void PrepareCards()
        {
            CollectAllSpawnedCards();
            GetCardsTransforms();
            ShufleCardsTransforms();
            SetFrontSideImagesOfCards();
        }

        private void CollectAllSpawnedCards()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                cards.Add(transform.GetChild(i).gameObject);
                cards[i].name = "" + i;
            }
            Debug.Log("Number of cards: " + cards.Count);
        }

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
        }

        List<Sprite> GetCardImages(int count)
        {
            // Load or generate card images based on the count
            List<Sprite> images = new List<Sprite>();
            // Add your sprites to the list here
            return images;
        }
    }
}