using UnityEngine;
using UnityEngine.EventSystems;

namespace Tesseract.Tooltip
{
    /// <summary>
    /// Attach to any UI element to show a tooltip on hover.
    /// </summary>
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, TextArea] private string _tooltipText;
        [SerializeField] private bool _useSlotPositioning = false;

        private RectTransform _rectTransform;

        public string TooltipText
        {
            get => _tooltipText;
            set => _tooltipText = value;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(_tooltipText)) return;
            if (!Singleton<TooltipManager>.HasInstance) return;

            if (_useSlotPositioning && _rectTransform != null)
                TooltipManager.Instance.ShowAtSlot(_tooltipText, _rectTransform);
            else
                TooltipManager.Instance.ShowAtMouse(_tooltipText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Singleton<TooltipManager>.HasInstance)
                TooltipManager.Instance.Hide();
        }
    }
}
