using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YagizEraslan.Duality
{
    public class OrientationHandler : MonoBehaviour
    {
        private CanvasScaler _canvasScaler;
        private float checkInterval = 1.0f;

        [Header("Portrait Resolution")]
        private int _portraitWidth = 1080;
        private int _portraitHeight = 1920;

        [Header("Landscape Resolution")]
        private int _landscapeWidth = 1920;
        private int _landscapeHeight = 1080;

        void Start()
        {
            _canvasScaler = GetComponent<CanvasScaler>();

#if UNITY_ANDROID || UNITY_IOS
            // Start the coroutine to periodically check the device orientation for mobile platforms
            StartCoroutine(CheckOrientationPeriodically());
#else
        // Set the resolution for landscape mode by default for non-mobile platforms
        _canvasScaler.referenceResolution = new Vector2(_landscapeWidth, _landscapeHeight);
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        IEnumerator CheckOrientationPeriodically()
        {
            while (true)
            {
                UpdateCanvasResolution();
                yield return new WaitForSeconds(checkInterval);
            }
        }

        void UpdateCanvasResolution()
        {
            if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                // Set the resolution for portrait mode
                _canvasScaler.referenceResolution = new Vector2(_portraitWidth, _portraitHeight);
            }
            else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                // Set the resolution for landscape mode
                _canvasScaler.referenceResolution = new Vector2(_landscapeWidth, _landscapeHeight);
            }
        }
#endif
    }
}