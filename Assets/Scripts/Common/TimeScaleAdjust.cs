using UnityEngine;

namespace Runtime.Utilities.Common
{
    public class TimeScaleAdjust : MonoBehaviour
    {
        public void AdjustTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}