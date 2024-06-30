using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] private TextMeshProUGUI turnsCountText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI bonusText;

        private int score;

        public int Score => score;

        public void CalculateScore(int matches, int turns, float timer)
        {
            int baseScore = CalculateBaseScore(matches);
            int turnPenalty = CalculateTurnPenalty(turns);
            int timePenalty = CalculateTimePenalty(timer);
            int totalImages = CardManager.Instance.TotalCards() / 2;
            int bonusScore = CalculateBonusScore(totalImages, turns, baseScore, turnPenalty, timePenalty);

            score = baseScore - turnPenalty - timePenalty + bonusScore;

            DisplayScoreInfo(turns, timer, totalImages, bonusScore);
        }

        private int CalculateBaseScore(int matches)
        {
            return matches * 100;
        }

        private int CalculateTurnPenalty(int turns)
        {
            return turns * 10;
        }

        private int CalculateTimePenalty(float timer)
        {
            return Mathf.FloorToInt(timer);
        }

        private int CalculateBonusScore(int totalImages, int turns, int baseScore, int turnPenalty, int timePenalty)
        {
            int bonusScore = 0;
            int bonusMultiplier = 2;

            if (totalImages == turns) // Perfect scoring
            {
                bonusText.text = $"Bonus: x{bonusMultiplier}";
                bonusScore = (baseScore - turnPenalty - timePenalty) * (bonusMultiplier - 1); // Subtracting penalties after multiplication
            }
            else if (totalImages + 1 == turns) // One mistake scoring
            {
                bonusScore = 200;
                bonusText.text = $"Bonus: +{bonusScore}";
            }
            else if (totalImages + 2 == turns) // Two mistakes scoring
            {
                bonusScore = 100;
                bonusText.text = $"Bonus: +{bonusScore}";
            }
            else
            {
                bonusText.text = $"Bonus: No bonus";
            }

            return bonusScore;
        }

        private void DisplayScoreInfo(int turns, float timer, int totalImages, int bonusScore)
        {
            Debug.Log($"Total Images: {totalImages}");
            Debug.Log($"Score: {score}");

            turnsCountText.text = $"Turns: {turns}/{totalImages}";
            timerText.text = $"Completed in {Mathf.FloorToInt(timer)} seconds";
            scoreText.text = $"Score: {score}";
        }
    }
}