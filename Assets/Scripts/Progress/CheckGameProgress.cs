using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    public class CheckGameProgress : MonoBehaviour
    {
        [SerializeField] private GameObject continuePanel;
        [SerializeField] private Button yesButton;

        void Start()
        {
            SaveFileChecker();
        }

        private void SaveFileChecker()
        {
            if (PlayerPrefs.HasKey("GamePhase"))
            {
                Debug.Log("Save file found!");
                continuePanel.SetActive(true);
                yesButton.onClick.AddListener(() => LoadPreviousLayoutType());
            }
            else
            {
                Debug.Log("No save file found!");
            }
        }

        public void LoadPreviousLayoutType()
        {
            int x = PlayerPrefs.GetInt("Layout_x");
            int y = PlayerPrefs.GetInt("Layout_y");
            
            yesButton.onClick.AddListener(() => { Debug.Log("Game resumes from previous session"); });
            yesButton.onClick.AddListener(() => UIManager.Instance.ShowGameplayCanvas());
            yesButton.onClick.AddListener(() => GridLayoutCreator.Instance.CreateGridLayout(x, y));
            yesButton.onClick.AddListener(() => continuePanel.SetActive(false));
        }
    }
}