using UnityEngine;

namespace Runtime.Utilities.Animators.FadableAdapters
{
    public class RectTransformSizeDeltaAdapter : IFadableAdapter
    {
        [SerializeField] private RectTransform _rectTransform;
        public float Value
        {
            get => _rectTransform.sizeDelta.x;
            set
            {
                _rectTransform.sizeDelta = new Vector2(value, value);
            }
        }
    }
}