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
        private string layoutType;

        private Button button;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            layoutType = $"{x}x{y}";

            button = GetComponent<Button>();
            button.onClick.AddListener(() => { Debug.Log($"{layoutType} layout type has chosen!"); });
            button.onClick.AddListener(() => UIManager.Instance.ShowGameplayCanvas());
            button.onClick.AddListener(() => ProgressManager.Instance.SaveLayoutType(x, y));
            button.onClick.AddListener(() => GridLayoutCreator.Instance.CreateGridLayout(x, y));
        }

        private void Start()
        {
            SetLevelButtonTextSettings();
        }

        private void SetLevelButtonTextSettings()
        {
            TextMeshProUGUI layoutText = GetComponentInChildren<TextMeshProUGUI>();
            layoutText.text = layoutType;
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