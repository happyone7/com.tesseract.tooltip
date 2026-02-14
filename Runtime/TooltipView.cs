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
            _tooltipRect.sizeDelta = textSize + _padding;
            if (_backgroundRect != null)
            {
                _backgroundRect.sizeDelta = textSize + _padding;
            }
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        public bool IsVisible => _canvasGroup.alpha > 0f;
    }
}
