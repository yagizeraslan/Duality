using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private GameObject _levelSelectionCanvas;
        [SerializeField] private GameObject _gameplayCanvas;
        [SerializeField] private GameObject _levelCompletedCanvas;

        public void ShowLevelSelectionCanvas()
        {
            _levelSelectionCanvas.SetActive(true);
            _gameplayCanvas.SetActive(false);
            _levelCompletedCanvas.SetActive(false);
        }

        public void ShowGameplayCanvas()
        {
            _levelSelectionCanvas.SetActive(false);
            _gameplayCanvas.SetActive(true);
            _levelCompletedCanvas.SetActive(false);
        }

        public void ShowLevelCompletedCanvas()
        {
            _levelSelectionCanvas.SetActive(false);
            _gameplayCanvas.SetActive(false);
            _levelCompletedCanvas.SetActive(true);
        }
    }
}