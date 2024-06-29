using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    public class LevelSelector : MonoBehaviour
    {
        [Header("Layout Values")]
        [SerializeField] private int x;
        [SerializeField] private int y;
        private string _layoutType;

        private Button _button;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _layoutType = $"{x}x{y}";
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => { Debug.Log($"{_layoutType} layout type has chosen!"); });
            _button.onClick.AddListener(() => UIManager.Instance.ShowGameplayCanvas());
            _button.onClick.AddListener(() => GridLayoutCreator.Instance.CreateGridLayout(x, y));
        }

        private void Start()
        {
            SetLevelButtonTextSettings();
        }

        private void SetLevelButtonTextSettings()
        {
            TextMeshProUGUI layoutText = GetComponentInChildren<TextMeshProUGUI>();
            layoutText.text = _layoutType;
            layoutText.color = Color.white;
            layoutText.fontStyle = FontStyles.Bold;
            layoutText.fontSize = 65;
        }

        public int GetXValue()
        {
            return x;
        }

        public int GetYValue()
        {
            return y;
        }
    }
}