using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YagizEraslan.Duality
{
    public class ProgressManager : MonoSingleton<ProgressManager>
    {
        public void SaveLayoutType(int x, int y)
        {
            PlayerPrefs.SetInt("Layout_x", x);
            PlayerPrefs.SetInt("Layout_y", y);
        }

        public void ClearProgress()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}