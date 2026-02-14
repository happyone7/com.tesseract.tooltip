using TMPro;
using UnityEngine;

namespace Tesseract.Tooltip
{
    /// <summary>
    /// Visual tooltip component. Attach to a UI panel with CanvasGroup and TMP text.
    /// </summary>
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _backgroundRect;
        [SerializeField] private RectTransform _tooltipRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Vector2 _padding = new Vector2(16f, 16f);

        public RectTransform RectTransform => _tooltipRect;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            if (_tooltipRect == null)
                _tooltipRect = GetComponent<RectTransform>();

            Hide();
        }

        public void SetText(string text)
        {
            _text.text = text;
            _text.ForceMeshUpdate();
            UpdateSize();
        }

        public void UpdateSize()
        {
            Vector2 textSize = _text.GetPreferredValues(_text.text);
            Vector2 size = textSize + _padding;
            _tooltipRect.sizeDelta = size;
            if (_backgroundRect != null)
            {
                _backgroundRect.sizeDelta = size;
            }
        }

        /// <summary>
        /// Get the actual size of the tooltip after text is set.
        /// </summary>
        public Vector2 GetSize()
        {
            return _tooltipRect.sizeDelta;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Hide()
        {
            if (_canvasGroup != null)
                _canvasGroup.alpha = 0f;
        }

        public bool IsVisible => _canvasGroup != null && _canvasGroup.alpha > 0f;
    }
}
