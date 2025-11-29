using UnityEngine;

namespace Runtime.Utilities.Animators.FadableAdapters
{
    public class CanvasGroupAdapter : IFadableAdapter
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        public float Value
        {
            get => _canvasGroup.alpha;
            set
            {
                _canvasGroup.alpha = value;
            }
        }
    }
}