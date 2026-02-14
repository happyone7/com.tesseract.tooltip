using Tesseract.Core;
using UnityEngine;

namespace Tesseract.Tooltip
{
    /// <summary>
    /// Manages tooltip display with mouse-follow and slot-based positioning.
    /// </summary>
    public class TooltipManager : Singleton<TooltipManager>
    {
        [SerializeField] private TooltipView _tooltip;
        [SerializeField] private Vector2 _offset = new Vector2(15f, -15f);

        private bool _isFollowingMouse;

        private void Update()
        {
            if (_isFollowingMouse && _tooltip != null)
            {
                Vector2 mousePosition = (Vector2)Input.mousePosition + _offset;
                mousePosition = ClampToScreen(mousePosition);
                _tooltip.transform.position = mousePosition;
            }
        }

        /// <summary>
        /// Show tooltip at mouse position (follows mouse).
        /// </summary>
        public void ShowAtMouse(string text)
        {
            if (_tooltip == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _isFollowingMouse = true;
        }

        /// <summary>
        /// Show tooltip at a fixed world position.
        /// </summary>
        public void ShowAtPosition(string text, Vector2 screenPosition)
        {
            if (_tooltip == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _tooltip.transform.position = ClampToScreen(screenPosition);
            _isFollowingMouse = false;
        }

        /// <summary>
        /// Show tooltip anchored to a RectTransform (e.g. a UI slot).
        /// Automatically positions left or right based on screen center.
        /// </summary>
        public void ShowAtSlot(string text, RectTransform slotTransform)
        {
            if (_tooltip == null || slotTransform == null) return;
            _tooltip.SetText(text);
            _tooltip.Show();
            _isFollowingMouse = false;

            Vector2 slotScreenPos = RectTransformUtility.WorldToScreenPoint(null, slotTransform.position);
            float screenCenter = Screen.width * 0.5f;

            // Use sizeDelta for accurate size after SetText/UpdateSize
            Vector2 tooltipSize = _tooltip.GetSize();
            float tooltipHalfWidth = tooltipSize.x * 0.5f;
            float slotHalfWidth = slotTransform.rect.width * slotTransform.lossyScale.x * 0.5f;
            float totalOffset = tooltipHalfWidth + slotHalfWidth;

            // If slot is on right side of screen, show tooltip on left (and vice versa)
            float xOffset = slotScreenPos.x >= screenCenter ? -totalOffset : totalOffset;

            Vector2 finalPos = new Vector2(
                slotScreenPos.x + xOffset,
                Screen.height * 0.5f
            );

            _tooltip.transform.position = ClampToScreen(finalPos);
        }

        /// <summary>
        /// Hide the tooltip.
        /// </summary>
        public void Hide()
        {
            if (_tooltip != null)
                _tooltip.Hide();
            _isFollowingMouse = false;
        }

        private Vector2 ClampToScreen(Vector2 position)
        {
            if (_tooltip == null) return position;

            // Use sizeDelta for accurate clamping regardless of pivot
            Vector2 size = _tooltip.GetSize();
            RectTransform rect = _tooltip.RectTransform;
            Vector2 pivot = rect.pivot;

            float minX = size.x * pivot.x;
            float maxX = Screen.width - size.x * (1f - pivot.x);
            float minY = size.y * pivot.y;
            float maxY = Screen.height - size.y * (1f - pivot.y);

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);

            return position;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                Hide();
        }
    }
}
