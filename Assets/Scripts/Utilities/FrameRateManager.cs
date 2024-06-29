using UnityEngine;

namespace YagizEraslan.Duality
{
    public class FrameRateManager : MonoBehaviour
    {
        private int _targetFrameRate = 60;

        void Start()
        {
            SetTargetFrameRate();
        }

        private void SetTargetFrameRate()
        {
            Application.targetFrameRate = _targetFrameRate;
            Debug.Log($"Target frame rate is set to {Application.targetFrameRate.ToString()}.");
        }
    }
}