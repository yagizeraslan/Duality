using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class GridLayoutCreator : MonoSingleton<GridLayoutCreator>
    {
        private int x; // Number of columns
        private int y; // Number of rows

        [SerializeField] private float spacing = 10f; // Spacing between UI Images

        [SerializeField] private Transform targetArea; // Parent object for the grid

        [SerializeField] private GameObject cardPrefab; // UI Image prefab to instantiate

        private float targetAreaSize = 1000f; // Size of the target area (1000x1000)

        public void CreateGridLayout(int x, int y)
        {
            // Calculate UI Image size based on the target area and spacing
            float imageSize = (targetAreaSize - (spacing * (Mathf.Max(x, y) - 1))) / Mathf.Max(x, y);

            // Calculate total grid size
            Vector2 gridSize = new Vector2(x * (imageSize + spacing) - spacing,
                                           y * (imageSize + spacing) - spacing);

            // Calculate starting position for the grid to be centered in the target area
            Vector3 startPosition = new Vector3(-gridSize.x / 2f + imageSize / 2f,
                                                gridSize.y / 2f - imageSize / 2f,
                                                0f);

            // Determine if the grid fits horizontally or vertically within the target area
            bool fitHorizontally = gridSize.x <= targetAreaSize;
            bool fitVertically = gridSize.y <= targetAreaSize;

            // If both orientations fit, use the calculated imageSize
            // If not, adjust the imageSize to fit within the target area
            if (!fitHorizontally || !fitVertically)
            {
                float maxGridSize = Mathf.Max(gridSize.x, gridSize.y);
                imageSize = (targetAreaSize - (spacing * (Mathf.Max(x, y) - 1))) / maxGridSize;
            }

            // Spawn UI Images in a grid pattern
            for (int row = 0; row < y; row++)
            {
                for (int col = 0; col < x; col++)
                {
                    Vector3 spawnPosition = startPosition + new Vector3(col * (imageSize + spacing), -row * (imageSize + spacing), 0f);

                    GameObject card = Instantiate(cardPrefab, targetArea);
                    card.transform.localPosition = spawnPosition;

                    // Adjust width and height of the UI Image
                    RectTransform rectTransform = card.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(imageSize, imageSize);
                }
            }

            // Creates whether previous layout type or new layout
            if (!PlayerPrefs.HasKey("GamePhase"))
            {
                PlayerPrefs.SetInt("GamePhase", 1);
                CardManager.Instance.CreateCards();
                Debug.Log("New layout has created!");
            }
            else
            {
                CardManager.Instance.LoadPreviousCards();
                Debug.Log("Previous layout has loaded!");
            }
        }
    }
}